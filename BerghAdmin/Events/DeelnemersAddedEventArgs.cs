namespace BerghAdmin.Events
{
    public class DeelnemersAddedEventArgs : EventArgs
    {
        public PersoonListItem[] Deelnemers { get; }

        public DeelnemersAddedEventArgs(PersoonListItem[] deelnemers)
        {
            Deelnemers = deelnemers;
        }
    }
}
