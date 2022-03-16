namespace BerghAdmin.ApplicationServices.Mail
{
    public record MailAddress(
        string Address,
        string? Name
    );
}
