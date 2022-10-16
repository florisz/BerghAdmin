namespace BerghAdmin.ApplicationServices.Mail;

public interface ISendMailService
{
    Task SendMail(MailMessage message, bool isSandboxMode = false);
    Task SendMail(string to, string from, string subject, string message, int? donateurId = null);
}