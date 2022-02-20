namespace BerghAdmin.Services;

public interface ISendMailService
{
    Task SendMail(string mailBody);
}