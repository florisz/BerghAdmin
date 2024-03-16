using System.IO;

namespace BerghAdmin.Services.Documenten
{
    public interface IPdfConverter
    {
        public Stream ConvertWordToPdf(Stream inputStream);
    }
}
