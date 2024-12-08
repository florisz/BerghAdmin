using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.Services.DateTimeProvider;
using BerghAdmin.Services.Documenten;
using BerghAdmin.Services.Sponsoren;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Facturen;

public class FactuurService : IFactuurService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private ILogger<FactuurService> _logger;
    private IDocumentMergeService _mergeService;
    private IAmbassadeurService _ambassadeurService;
    private IPdfConverter _pdfConverter;

    public FactuurService(ApplicationDbContext dbContext, 
                            IDateTimeProvider dateTimeProvider,
                            IDocumentMergeService mergeService,
                            IPdfConverter pdfConverter,
                            IAmbassadeurService ambassadeurService,
                            ILogger<FactuurService> logger)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _mergeService = mergeService;
        _pdfConverter = pdfConverter;
        _ambassadeurService = ambassadeurService;
        _logger = logger;
        logger.LogDebug($"FactuurService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
    }
    public async Task DeleteFactuurAsync(Factuur factuur)
    {
        _logger.LogDebug($"Delete factuur with id:{factuur.Id}");

        if (factuur != null)
        {
            _dbContext.Facturen?.Remove(factuur);
            await _dbContext.SaveChangesAsync();

            _logger.LogDebug($"Factuur with nummer {factuur.Nummer} deleted");
        }
    }

    public Task<List<Factuur>> GetFacturenAsync(int jaar)
        => _dbContext.Facturen!
            .Where(f => f.Datum.Year == jaar)
            .OrderBy(f => f.Nummer)
            .ToListAsync();


    public async Task<Factuur?> GetFactuurByNummerAsync(int nummer)
        => await _dbContext
                    .Facturen!
                    .FirstOrDefaultAsync(f => f.Nummer == nummer);

    public async Task<Factuur?> GetFactuurByNummerWithPdfAsync(int nummer)
        => await _dbContext
                    .Facturen!
                    .Include(t => t.FactuurTekst)
                    .FirstOrDefaultAsync(f => f.Nummer == nummer);

    public async Task<Factuur?> GetFactuurByIdAsync(int id)
        => await _dbContext
                    .Facturen!
                    .FirstOrDefaultAsync(f => f.Id == id);

    public async Task<Factuur?> GetFactuurByIdWithPdfAsync(int id)
        => await _dbContext
                    .Facturen!
                    .Include(t => t.FactuurTekst)
                    .FirstOrDefaultAsync(f => f.Id == id);

    public async Task<Factuur?> GetNewFactuurAsync(Ambassadeur ambassadeur)
        => await GetNewFactuurAsync(_dateTimeProvider.Now, ambassadeur);

    public async Task<Factuur?> GetNewFactuurAsync(DateTime dateTime, Ambassadeur ambassadeur)
        => await GetNewFactuurAsync(GetNextFactuurNummer(), dateTime, ambassadeur);

    public async Task<Factuur?> GetNewFactuurAsync(int nummer, DateTime dateTime, Ambassadeur ambassadeur)
    {
        int tries = 0;
        bool returnValue = false;
        Factuur factuur = null!;

        while (!returnValue)
        {
            factuur = new Factuur(nummer, dateTime, ambassadeur.Id);
            returnValue = await SaveFactuurAsync(factuur, ambassadeur);

            if (!returnValue && tries++ > 3)
            {
                throw new ApplicationException("Could not generate new factuur, tried it 3 times");
            }
        }

        return await GetFactuurByNummerAsync(nummer);
    }

    public async Task<bool> SaveFactuurAsync(Factuur factuur, Ambassadeur ambassadeur)
    {
        _logger.LogDebug($"SaveAsync factuur with nummer {factuur.Nummer}");

        var debugView = _dbContext.ChangeTracker.DebugView.LongView;
        if (factuur.Id == 0)
        {
            _dbContext
                .Facturen?
                .Add(factuur);

            _logger.LogInformation($"Factuur with nummer {factuur.Nummer} was added");
        }
        else
        {
            _dbContext
                .Facturen?
                .Update(factuur);

            _logger.LogInformation($"Factuur with nummer {factuur.Nummer} was updated");
        }
        ambassadeur.Facturen.Add(factuur);
        await _ambassadeurService.SaveAsync(ambassadeur);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task MaakFactuurVoorAmbassadeur(string templateName, Ambassadeur ambassadeur)
    {
        var template = _mergeService.GetMergeTemplateByName(templateName);

        var factuur = await GetNewFactuurAsync(ambassadeur);

        var mergeDictionary = new Dictionary<string, string>();
        mergeDictionary.Add("AmbassadeurNaam", ambassadeur.Naam);
        mergeDictionary.Add("ContactpersoonAanhef", ambassadeur.ContactPersoon1.Aanhef);
        mergeDictionary.Add("ContactpersoonVoornaam", ambassadeur.ContactPersoon1.Voornaam);
        mergeDictionary.Add("ContactpersoonAchternaam", ambassadeur.ContactPersoon1.Achternaam);
        mergeDictionary.Add("AmbassadeurAdres", ambassadeur.Adres);
        mergeDictionary.Add("AmbassadeurPostcode", ambassadeur.Postcode);
        mergeDictionary.Add("AmbassadeurWoonplaats", ambassadeur.Plaats);
        mergeDictionary.Add("Dagtekening", _dateTimeProvider.Now.ToString("d"));
        mergeDictionary.Add("FactuurNummer", factuur!.FactuurNummer);
        mergeDictionary.Add("DebiteurNummer", ambassadeur.DebiteurNummer);
        mergeDictionary.Add("HuidigeDatum", _dateTimeProvider.Now.ToString("d"));
        mergeDictionary.Add("FactuurBedrag", string.Format("€ {0:N2} Euro", ambassadeur.ToegezegdBedrag));
        mergeDictionary.Add("FactuurTotaalBedrag", string.Format("€ {0:N2} Euro", ambassadeur.ToegezegdBedrag));

        // Convert template Document content to MemoryStream
        using var templateStream = new MemoryStream(template.Content);
        using var mergedStream = _mergeService.Merge(templateStream, mergeDictionary);
        mergedStream.Position = 0;
        using var pdfStream = _pdfConverter.ConvertWordToPdf(mergedStream);

        await SaveStreamAsFactuur(pdfStream, ambassadeur, factuur);

        return;
    }

    private int GetNextFactuurNummer()
    {
        int nummer;

        // generate next consecutive invoice number based on last invoice number
        var laatsteFactuur = _dbContext.Facturen!.OrderByDescending(f => f.Nummer).FirstOrDefault();
        if (laatsteFactuur == null)
        {
            nummer = 1;
        }
        else
        {
            nummer = laatsteFactuur!.Nummer + 1;
        }

        return nummer;
    }

    private async Task SaveStreamAsFactuur(Stream factuurStream, Ambassadeur ambassadeur, Factuur factuur)
    {
        factuurStream.Position = 0;
        factuur.Omschrijving = "Factuur voor " + ambassadeur.Naam + "; datum: " + factuur.Datum;
        factuur.Bedrag = ambassadeur.ToegezegdBedrag;
        factuur.FactuurStatus = FactuurStatusEnum.TeVersturen;
        factuur.FactuurType = FactuurTypeEnum.Pdf;
        factuur.FactuurTekst = new Document()
        {
            Content = await StreamToByteArray(factuurStream),
            ContentType = ContentTypeEnum.Factuur,
            DocumentType = DocumentTypeEnum.Pdf,
            TemplateType = TemplateTypeEnum.Ambassadeur,
            IsMergeTemplate = false,
            Owner = "BerghAdmin",
            Name = "Factuur " + factuur.FactuurNummer + ".pdf",
            Created = _dateTimeProvider.Now
        };

        ambassadeur.Facturen.Add(factuur);
        
        await SaveFactuurAsync(factuur, ambassadeur);
    }

    private async Task<byte[]> StreamToByteArray(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

}
