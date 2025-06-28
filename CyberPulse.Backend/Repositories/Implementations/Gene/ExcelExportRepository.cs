using ClosedXML.Excel;
using CyberPulse.Backend.Data;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Shared.Entities.Chipp;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Gene;

public class ExcelExportRepository :  IExcelExportRepository
{
    private readonly ApplicationDbContext _context;

    public ExcelExportRepository(ApplicationDbContext context) 
    {
        _context = context;
    }

    public async Task<(int sheetsProcessed, int rowsProcessed)> ProcessExcelAsync(Stream fileStream)
    {
        using var workbook = new XLWorkbook(fileStream);
        int totalSheets = 0;
        int totalRows = 0;
        bool rowsExist = false;
        bool rowExist = true;

        var triningLevels = await _context.TriningLevels.ToListAsync();
        var priorityBets = await _context.PriorityBets.ToListAsync();

        if (_context.ChipPrograms.Any())
        {
            rowsExist = true;
        }

        foreach (var worksheet in workbook.Worksheets)
        {
            totalSheets++;

            worksheet.RangeUsed()!.SetAutoFilter(false);
            //var dataTable = worksheet.RangeUsed()!.AsTable().AsNativeDataTable();
            var dataTable = worksheet.Tables.Table("Tabla1").AsNativeDataTable();

            for (int rowNum = 1; rowNum <= dataTable.Rows.Count; rowNum++)
            {
                var row = dataTable.Rows[rowNum - 1];

                var PRF_CODIGO = row["PRF_CODIGO"].ToString();
                var version = row["PRF_VERSION"].ToString();
                var denominacion = row["PRF_DENOMINACION"].ToString();
                var Tipo = row["TIPO DE FORMACION"].ToString();
                var level = row["NIVEL DE FORMACION"].ToString();
                var Duracion = row["PRF_DURACION_MAXIMA"];
                var Fecha = row["Fecha Activo (En Ejecución) "];
                var ApoyoFic = row["PRF_APOYO_FIC"];
                var Apuestas = row["APUESTAS PRIORITARIAS"].ToString();
                var alamedida = row["PRF_ALAMEDIDA"].ToString();

                if (string.IsNullOrWhiteSpace(PRF_CODIGO) ||
                    string.IsNullOrWhiteSpace(version) ||
                    string.IsNullOrWhiteSpace(denominacion) ||
                    string.IsNullOrWhiteSpace(Tipo) ||
                    string.IsNullOrWhiteSpace(level) ||
                    string.IsNullOrWhiteSpace(Duracion.ToString()) ||
                    string.IsNullOrWhiteSpace(Fecha.ToString()) ||
                    string.IsNullOrWhiteSpace(Apuestas))
                {
                    continue;
                }

                if (rowsExist)
                {
                    var response = await _context.ChipPrograms.Where(x => x.Code == PRF_CODIGO).FirstOrDefaultAsync();

                    if (response != null)
                    {
                        rowExist = false;
                    }
                }

                if (rowExist)
                {
                    var triningLevel = triningLevels.FirstOrDefault(x => x.Name.ToLower() == level!.ToLower());
                    var priorityBet = priorityBets.FirstOrDefault(x => x.Name.ToUpper() == Apuestas!.ToUpper());
                    if (triningLevel != null && priorityBet != null)
                    {

                        totalRows++;

                        _context.ChipPrograms.Add(new ChipProgram
                        {
                            Code = PRF_CODIGO!,
                            Version=int.Parse( version),
                            Designation=denominacion,
                            Duration = int.Parse(Duracion.ToString()!),
                            PriorityBetId = priorityBet.Id,
                            StartDate = DateTime.Parse(Fecha.ToString()!),
                            SupportFic = (ApoyoFic.ToString() == "NO" ? false : true),
                            TriningLevelId = triningLevel.Id,
                            TypeOfTraining = Tipo,
                            WingMeasure = (alamedida == "NO" ? true : false)
                        });
                        // Guardar cada 100 filas para mejor performance
                        if (totalRows % 100 == 0)
                        {
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    rowExist = true;
                }
            }
        }

        if (totalRows % 100 != 0)
        {
            // Guardar cualquier dato pendiente
            await _context.SaveChangesAsync();
        }

        return (totalSheets, totalRows);
    }
}
