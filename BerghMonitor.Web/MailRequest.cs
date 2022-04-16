namespace BerghMonitor.Web;

public record MailRequest(string Subject, string To, string Body);
