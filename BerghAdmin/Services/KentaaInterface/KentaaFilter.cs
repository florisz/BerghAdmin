namespace BerghAdmin.Services.KentaaInterface;

public class KentaaFilter : IKentaaFilter
{
    public KentaaFilter(string query)
    {
        Query = query;
        StartAt = 0;
        PageSize = 50;
    }

    public KentaaFilter(string query, int startAt, int pageSize)
    {
        Query = query;
        StartAt = startAt;
        PageSize = pageSize;
    }

    public string Query { get; set; }
    public int StartAt { get; set; }
    public int PageSize { get; set; }

    public KentaaFilter NextPage()
    {
        return new KentaaFilter(Query, StartAt + PageSize, PageSize);
    }

    public string Build()
    {
        return $"jql={Query}&startAt={StartAt}&pageSize={PageSize}";
    }
}
