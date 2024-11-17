namespace BerghAdmin.Data;

public enum FactuurVerzendwijzeEnum
{
    Mail,
    Post,
    Onbekend
}

public class FactuurVerzendwijze
{
    public FactuurVerzendwijzeEnum FactuurVerzendwijzeValue { get; set; }
    public string FactuurVerzendwijzeText { get; set; } = "";
}

public static class FactuurVerzendwijzeService
{
    public static FactuurVerzendwijze[] GetFactuurVerzendwijzeValues()
    =>
        [
            new FactuurVerzendwijze() { FactuurVerzendwijzeValue = FactuurVerzendwijzeEnum.Mail,
                                        FactuurVerzendwijzeText = FactuurVerzendwijzeEnum.Mail.ToString() },
            new FactuurVerzendwijze() { FactuurVerzendwijzeValue = FactuurVerzendwijzeEnum.Post,
                                        FactuurVerzendwijzeText = FactuurVerzendwijzeEnum.Post.ToString() },
            new FactuurVerzendwijze() { FactuurVerzendwijzeValue = FactuurVerzendwijzeEnum.Onbekend,
                                        FactuurVerzendwijzeText = FactuurVerzendwijzeEnum.Onbekend.ToString() },
        ];
}

