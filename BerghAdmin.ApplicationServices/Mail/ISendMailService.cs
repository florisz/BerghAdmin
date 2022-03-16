namespace BerghAdmin.ApplicationServices.Mail;

public interface ISendMailService
{
    Task<bool> SendMail(MailMessage message);
}