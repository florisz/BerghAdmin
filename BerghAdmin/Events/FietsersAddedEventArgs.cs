namespace BerghAdmin.Events
{
    public class FietsersAddedEventArgs : EventArgs
    {
        public PersoonListItem[] Fietsers{ get; }

        public FietsersAddedEventArgs(PersoonListItem[] fietsers)
        {
            Fietsers = fietsers;
        }
    }
}
