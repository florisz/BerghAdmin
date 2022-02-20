namespace BerghAdmin.ApplicationServices.Mail;
public interface IMailSenderService
{
    Task SendMailAsync(MailAddress from, IEnumerable<MailAddress> to, Stream mailBody);
}
