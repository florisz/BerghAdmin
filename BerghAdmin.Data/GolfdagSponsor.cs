namespace BerghAdmin.Data;

public class GolfdagSponsor : Sponsor
{
    public ICollection<Golfdag> GolfdagenGesponsored { get; set; } = new List<Golfdag>();
}
