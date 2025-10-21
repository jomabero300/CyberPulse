namespace CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;

public interface IExcelExportUnitOfWork
{
    Task<(int sheetsProcessed, int rowsProcessed)> ProcessExcelAsync(Stream fileStream);
}
