namespace BerghAdmin.Events
{
    public class MailAddressUpdatedEventArgs : EventArgs
    {
        public MailAddress? MailAddress { get; }

        public MailAddressUpdatedEventArgs(MailAddress? mailAddress)
        {
            MailAddress = mailAddress;
        }
    }
}
