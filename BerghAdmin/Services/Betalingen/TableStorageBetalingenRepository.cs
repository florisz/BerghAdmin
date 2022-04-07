using Azure.Data.Tables;

using System.Text.Json;

namespace BerghAdmin.Services.Betalingen;

public class TableStorageBetalingenRepository : IBetalingenRepository
{
    private TableClient tableClient;

    public TableStorageBetalingenRepository()
    {
        tableClient = new TableClient(
            new Uri("https://berghuatstorage.table.core.windows.net"),
            "betalingen",
            new TableSharedKeyCredential("berghuatstorage", "HqU3OmMYf7lQCY7mvySH4bYwhj81RzJV6lS4H22chM6b17S5JH8VjBbmAByNA8b9Gb7Iii4Voy/bmih5sfl35Q=="));
    }

    public void Add(Betaling betaling)
    {
        var entity = new TableEntity(betaling.Code, betaling.Volgnummer)
        {
            { "data", JsonSerializer.Serialize(betaling) }
        };

        var response = tableClient.AddEntity(entity);
    }

    public void Update(Betaling betaling)
    {
    }

    public Betaling? GetByVolgnummer(string volgNummer)
    {
        return null;
    }
    public IEnumerable<Betaling>? GetAll()
    {
        var rows = tableClient.Query<TableEntity>();
        return rows.Select(r => JsonSerializer.Deserialize<Betaling>((string) r["data"])!);
    }
}
