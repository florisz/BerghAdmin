namespace BerghAdmin.ApplicationServices.Mail
{
    public class MailMessage
    {
        public MailAddress From { get; set; }
        public List<MailAddress> To { get; set; }
        public List<MailAddress> Cc { get; set; }
        public List<MailAddress> Bcc { get; set; }
        public string Subject { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
    }
}
