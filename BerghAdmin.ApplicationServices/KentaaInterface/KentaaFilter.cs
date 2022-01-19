namespace BerghAdmin.ApplicationServices.KentaaInterface;

public class KentaaFilter : IKentaaFilter
{
    public KentaaFilter()
    {
        StartAt = 1;
        PageSize = 25;
    }

    public KentaaFilter(int startAt, int pageSize)
    {
        StartAt = startAt;
        PageSize = pageSize;
    }

    public int StartAt { get; set; }
    public int PageSize { get; set; }

    public KentaaFilter NextPage()
    {
        return new KentaaFilter(StartAt + 1, PageSize);
    }

    public string Build()
    {
        return $"page={StartAt};per_page={PageSize}";
    }
}
