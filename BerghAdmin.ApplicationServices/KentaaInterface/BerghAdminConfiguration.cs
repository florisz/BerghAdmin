namespace BerghAdmin.ApplicationServices.KentaaInterface
{
    public class BerghAdminConfiguration
    {
        public Uri Host { get; set; } = new Uri("https://localhost:5001");
        public int Port { get; set; } = 5001;
    }
}