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
    private readonly ILogger<FactuurService> _logger;
    private readonly IDocumentMergeService _mergeService;
    private readonly IAmbassadeurService _ambassadeurService;
    private readonly IFactuurNummerService _factuurNummerGenerator;
    private readonly IPdfConverter _pdfConverter;

    public FactuurService(ApplicationDbContext dbContext, 
                            IDateTimeProvider dateTimeProvider,
                            IDocumentMergeService mergeService,
                            IPdfConverter pdfConverter,
                            IAmbassadeurService ambassadeurService,
                            IFactuurNummerService factuurNummerGenerator,
                            ILogger<FactuurService> logger)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _mergeService = mergeService;
        _pdfConverter = pdfConverter;
        _ambassadeurService = ambassadeurService;
        _factuurNummerGenerator = factuurNummerGenerator;
        _logger = logger;
        logger.LogDebug("{Message}", $"FactuurService created; threadid={Environment.CurrentManagedThreadId}, dbcontext={dbContext.ContextId}");
    }
    public async Task DeleteFactuurAsync(Factuur factuur)
    {
        _logger.LogDebug("{Message}", $"Delete factuur with id:{factuur.Id}");

        if (factuur != null)
        {
            _dbContext.Facturen?.Remove(factuur);
            await _dbContext.SaveChangesAsync();

            _logger.LogDebug("{Message}", $"Factuur with nummer {factuur.Nummer} deleted");
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
        => await GetNewFactuurAsync(_factuurNummerGenerator.GetNextNummer(), dateTime, ambassadeur);

    public async Task<Factuur?> GetNewFactuurAsync(int nummer, DateTime dateTime, Ambassadeur ambassadeur)
    {
        int tries = 0;
        bool returnValue = false;
        Factuur factuur;

        while (!returnValue)
        {
            factuur = new Factuur(nummer, dateTime, ambassadeur.Id, "A");
            returnValue = await SaveFactuurAsync(factuur, ambassadeur);

            if (!returnValue && tries++ > 3)
            {
                throw new ApplicationException("Could not generate new factuur, tried it 3 times");
            }
        }

        return await GetFactuurByNummerAsync(nummer);
    }

    public async Task<bool> SaveFactuurAsync(Factuur factuur)
    { 
        return await SaveFactuurAsync(factuur, null);
    }

    public async Task<bool> SaveFactuurAsync(Factuur factuur, Ambassadeur? ambassadeur)
    {
        _logger.LogDebug("{Message}", $"SaveAsync factuur with nummer {factuur.Nummer}");

        if (factuur.Id == 0)
        {
            _dbContext
                .Facturen?
                .Add(factuur);

            _logger.LogInformation("{Message}", $"Factuur with nummer {factuur.Nummer} was added");
        }
        else
        {
            _dbContext
                .Facturen?
                .Update(factuur);

            _logger.LogInformation("{Message}", $"Factuur with nummer {factuur.Nummer} was updated");
        }
        if (ambassadeur != null)
        {
            ambassadeur.Facturen.Add(factuur);
            await _ambassadeurService.SaveAsync(ambassadeur);
        }
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task MaakFactuurVoorAmbassadeur(string templateName, Ambassadeur ambassadeur)
    {
        if (ambassadeur is null)
        {
            throw new ArgumentNullException(nameof(ambassadeur));
        }

        var template = _mergeService.GetMergeTemplateByName(templateName);

        var factuur = await GetNewFactuurAsync(ambassadeur);

        var mergeDictionary = new Dictionary<string, string?>
        {
            { "AmbassadeurNaam", ambassadeur.Naam },
            { "ContactpersoonAanhef", ambassadeur.ContactPersoon1!.Aanhef },
            { "ContactpersoonVoornaam", ambassadeur.ContactPersoon1!.Voornaam },
            { "ContactpersoonAchternaam", ambassadeur.ContactPersoon1!.Achternaam },
            { "AmbassadeurAdres", ambassadeur.Adres },
            { "AmbassadeurPostcode", ambassadeur.Postcode },
            { "AmbassadeurWoonplaats", ambassadeur.Plaats },
            { "Dagtekening", _dateTimeProvider.Now.ToString("d") },
            { "FactuurNummer", factuur!.FactuurNummer },
            { "DebiteurNummer", ambassadeur.DebiteurNummer },
            { "HuidigeDatum", _dateTimeProvider.Now.ToString("d") },
            { "FactuurBedrag", string.Format("€ {0:N2} Euro", ambassadeur.ToegezegdBedrag) },
            { "FactuurTotaalBedrag", string.Format("€ {0:N2} Euro", ambassadeur.ToegezegdBedrag) }
        };

        // Convert template Document content to MemoryStream
        using var templateStream = new MemoryStream(template.Content);
        using var mergedStream = _mergeService.Merge(templateStream, mergeDictionary);
        mergedStream.Position = 0;
        using var pdfStream = _pdfConverter.ConvertWordToPdf(mergedStream);

        await SaveStreamAsFactuur(pdfStream, ambassadeur, factuur);

        return;
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

    static private async Task<byte[]> StreamToByteArray(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

}
