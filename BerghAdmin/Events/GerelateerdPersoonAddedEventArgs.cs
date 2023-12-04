namespace BerghAdmin.Events
{
    public class GerelateerdPersoonAddedEventArgs : EventArgs
    {
        public PersoonListItem GerelateerdPersoon { get; }
        public string PersoonType { get; }

        public GerelateerdPersoonAddedEventArgs(PersoonListItem gerelateerdPersoon, string persoonType)
        {
            GerelateerdPersoon = gerelateerdPersoon;
            PersoonType = persoonType;
        }
    }
}
