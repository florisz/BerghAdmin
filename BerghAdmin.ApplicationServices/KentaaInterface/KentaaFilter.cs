namespace BerghAdmin.ApplicationServices.KentaaInterface;

public class KentaaFilter : IKentaaFilter
{
    public KentaaFilter()
    {
        StartAt = 1;
        PageSize = 25;
    }

    public KentaaFilter(int startAt, 
                        int pageSize,
                        DateTime? createdAfter = null,
                        DateTime? createdBefore = null, 
                        DateTime? updatedAfter = null,
                        DateTime? updatedBefore = null )
    {
        StartAt = startAt;
        PageSize = pageSize;
        CreatedAfter = createdAfter;
        CreatedBefore = createdBefore;
        UpdatedAfter = updatedAfter;
        UpdatedBefore = updatedBefore;
    }

    public int StartAt { get; set; }
    public int PageSize { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
    public DateTime? UpdatedAfter { get; set; }
    public DateTime? UpdatedBefore { get; set; }

    public KentaaFilter NextPage()
    {
        return new KentaaFilter(StartAt+1, 
                                PageSize, 
                                this.CreatedAfter,
                                this.CreatedBefore,
                                this.UpdatedAfter,
                                this.UpdatedBefore);
        return new KentaaFilter(StartAt + 1, PageSize);
    }

    public string Build()
    {
        var queryString = $"page={StartAt};per_page={PageSize}";
        if (CreatedAfter != null)
        {
            queryString = $"{queryString};created_after={ToSearchDate((DateTime) CreatedAfter)}";
        }

        if (CreatedBefore != null)
        {
            queryString = $"{queryString};created_before={ToSearchDate((DateTime) CreatedBefore)}";
        }

        if (UpdatedAfter != null)
        {
            queryString = $"{queryString};updated_after={ToSearchDate((DateTime) UpdatedAfter)}";
        }

        if (UpdatedBefore != null)
        {
            queryString = $"{queryString};updated_before={ToSearchDate((DateTime) UpdatedBefore)}";
        }

        return queryString;
    }

    public string ToSearchDate(DateTime dt)
    {
        return dt.ToString("yyyy-MM-ddTHH:mm:ssZ");
    }
}
