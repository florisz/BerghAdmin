using System.IO;

namespace BerghAdmin.Services
{
    public interface IPdfConverter
    {
        public Stream ConvertWordToPdf(Stream inputStream);
    }
}
