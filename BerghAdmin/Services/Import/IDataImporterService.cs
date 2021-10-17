using System;
using System.IO;

namespace BerghAdmin.Services.Import
{
    public interface IDataImporterService
    {
        void ImportData(Stream csvData);
    }
}
