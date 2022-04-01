namespace BerghAdmin.ApplicationServices.Mail;

public interface ISendMailService
{
    Task SendMailAsync(MailMessage message, bool isSandboxMode = false);
}