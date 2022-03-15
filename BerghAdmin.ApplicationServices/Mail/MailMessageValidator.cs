namespace BerghAdmin.ApplicationServices.Mail
{
    public static class MailMessageValidator
    {
        public static Dictionary<string, List<string>> Validate(this MailMessage message)
        {
            Dictionary<string, List<string>> validationProblems = new();
            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                validationProblems.AddProblem(nameof(message.Subject), "Subject is required");
            }
            if (string.IsNullOrWhiteSpace(message.From?.Address))
            {
                validationProblems.AddProblem(nameof(message.From), "Sender is required");
            }
            if (message.From != null && !message.From.Address.EndsWith("berghinhetzadel.nl"))
            {
                validationProblems.AddProblem(nameof(message.From), "Domain of sender's email address is not 'berghinhetzadel.nl'");
            }
            if ((message.To == null || !message.To.Any()) &&
                (message.Cc == null || !message.Cc.Any()) &&
                (message.Bcc == null || !message.Bcc.Any()))
                
            {
                validationProblems.AddProblem(nameof(message.To), "At least one of To, CC or BCC must be specified");
            }

            return validationProblems;
        }

        public static void AddProblem(this Dictionary<string, List<string>> validationProblems, string key, string description)
        {
            if (!validationProblems.ContainsKey(key))
            {
                validationProblems[key] = new List<string>();
            }
            validationProblems[key].Add(description);
        }
    }
}
