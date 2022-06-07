namespace BerghAdmin.ApplicationServices.Mail;

public class MailMessage
{
    public MailAddress From { get; set; } = new MailAddress(string.Empty, string.Empty);
    public string Subject { get; set; } = string.Empty;
    public List<MailAddress> To { get; set; } = new();
    public List<MailAddress> Cc { get; set; } = new();
    public List<MailAddress> Bcc { get; set; } = new();
    public string TextBody { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public List<MailAttachment> InlinedAttachments { get; set; } = new();

    private readonly Dictionary<string, Problem> validationProblems = new();

    public Dictionary<string, Problem> Validate()
    {
        if (string.IsNullOrWhiteSpace(this.Subject))
        {
            AddProblem(nameof(this.Subject), "Subject is required");
        }

        if (string.IsNullOrWhiteSpace(this.From.Address))
        {
            AddProblem(nameof(this.From), "Sender is required");
        }

        if (!this.From.Address.EndsWith("berghinhetzadel.nl"))
        {
            AddProblem(nameof(this.From.Address), "Domain of sender's email address is not 'berghinhetzadel.nl'");
        }

        if (!this.To.Any())
        {
            AddProblem(nameof(this.To), "At least one To address must be specified");
        }

        return validationProblems;
    }

    private void AddProblem(string key, string description)
    {
        validationProblems[key] = new Problem(key, description);
    }


    public record Problem(string Key, string Message);
}
