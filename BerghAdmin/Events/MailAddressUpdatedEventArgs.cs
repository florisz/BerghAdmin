namespace BerghAdmin.Events
{
    public class MailAddressUpdatedEventArgs : EventArgs
    {
        public MailAddress MailAddress { get; set; }

        public MailAddressUpdatedEventArgs(MailAddress mailAddress)
        {
            MailAddress = mailAddress;
        }
    }
}
