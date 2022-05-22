namespace BerghAdmin.ApplicationServices.Mail
{
    public class MailAddress
    {
        public string Address { get; set; }
        public string Name { get; set; }

        public MailAddress(string address, string name)
        {
            Address = address;
            Name = name;
        }
    }
}
