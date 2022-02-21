#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace BerghAdmin.ApplicationServices.Mail;

public class MailAddress
{
    string EmailAddres { get; set; }
    string Name { get; set; }
}

public class MailSenderService : IMailSenderService
{
    public Task SendMailAsync(MailAddress from, IEnumerable<MailAddress> to, Stream mailBody)
    {
        throw new NotImplementedException();
    }
}
