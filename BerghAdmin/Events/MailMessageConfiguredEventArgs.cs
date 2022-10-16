namespace BerghAdmin.Events
{
    public class MailMessageConfiguredEventArgs : EventArgs
    {
        public MailMessage Message { get; }

        public MailMessageConfiguredEventArgs(MailMessage message)
        {
            Message = message;
        }
    }
}
