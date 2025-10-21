namespace CyberPulse.Backend.Repositories.Interfaces.Gene;

public interface IExcelExportRepository
{
    Task<(int sheetsProcessed, int rowsProcessed)> ProcessExcelAsync(Stream fileStream);
}