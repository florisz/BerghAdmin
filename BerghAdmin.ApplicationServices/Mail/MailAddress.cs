namespace BerghAdmin.ApplicationServices.Mail
{
    public class MailAddress
    {
        public string Address { get; set; }
        public string? Name { get; set; }
        public int? DonateurId { get; set; }

        public MailAddress(string address, string? name, int? donateurId = null)
        {
            Address = address;
            Name = name;
            DonateurId = donateurId;
        }
    }
}
