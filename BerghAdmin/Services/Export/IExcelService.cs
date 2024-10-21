using Syncfusion.Blazor.Grids;

namespace BerghAdmin.Services.Export;

public interface IExcelService
{
    Task<bool> ExportPersonenAsync(string path);
}
