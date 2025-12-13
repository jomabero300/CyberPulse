using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Controllers.Gene;

//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class ExcelExportsController : ControllerBase
{
    private readonly IExcelExportUnitOfWork _excelExportUnitOfWork;
    private readonly ILogger<ExcelExportsController> _logger;
    public ExcelExportsController(IExcelExportUnitOfWork excelExportUnitOfWork, ILogger<ExcelExportsController> logger)
    {
        _excelExportUnitOfWork = excelExportUnitOfWork;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No se ha proporcionado un archivo válido.");
        }

        if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Solo se permiten archivos Excel (.xlsx).");
        }

        try
        {
            using var stream = file.OpenReadStream();
            var (sheets, rows) = await _excelExportUnitOfWork.ProcessExcelAsync(stream);

            return Ok(new
            {
                Message = "Archivo procesado correctamente",
                SheetsProcessed = sheets,
                RowsProcessed = rows
            });
        }
        catch (DbUpdateException)
        {

            return StatusCode(500, $"Error interno: existen registro duplicados..");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar el archivo Excel");
            return StatusCode(500, $"Error interno: {ex.Message}");
        }

    }
}
