using System.IO;

namespace bihz.kantoorportaal.Services
{
    public interface IPdfConverter
    {
        public Stream ConvertWordToPdf(Stream inputStream);
    }
}
