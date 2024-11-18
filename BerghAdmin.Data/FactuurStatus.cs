namespace BerghAdmin.Data;

public enum FactuurStatusEnum
{
    TeVersturen,
    Verzonden,
    Betaald,
    NietBetaald,
    Achterstallig,
    Gecrediteerd
}

public class FactuurStatus
{
    public FactuurStatusEnum FactuurStatusValue { get; set; }
    public string FactuurStatusText { get; set; } = "";
}

public static class FactuurStatusService
{
    public static FactuurStatus[] GetFactuurStatusValues()
    =>
        [
            new FactuurStatus() { FactuurStatusValue = FactuurStatusEnum.TeVersturen,
                                  FactuurStatusText = "Te versturen" },
            new FactuurStatus() { FactuurStatusValue = FactuurStatusEnum.Verzonden,
                                  FactuurStatusText = "Verzonden" },
            new FactuurStatus() { FactuurStatusValue = FactuurStatusEnum.Betaald,
                                  FactuurStatusText = "Betaald" },
            new FactuurStatus() { FactuurStatusValue = FactuurStatusEnum.NietBetaald,
                                  FactuurStatusText = "Niet betaald" },
            new FactuurStatus() { FactuurStatusValue = FactuurStatusEnum.Achterstallig,
                                  FactuurStatusText = "Achterstallig" },
            new FactuurStatus() { FactuurStatusValue = FactuurStatusEnum.Gecrediteerd,
                                  FactuurStatusText = "Gecrediteerd" },
        ];
}

