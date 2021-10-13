using System;
using System.IO;

namespace BerghAdmin.Services
{
    public interface IDataImporterService
    {
        void ImportData(Stream csvData);
    }
}
