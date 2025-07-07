using CyberPulse.Shared.Entities.Gene;
using QuestPDF.Fluent;

namespace CyberPulse.Backend.Helpers;

public static class ReporteService
{
    public static byte[] GenerarPdf(List<Producto> productos)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Header().Text("Reporte de Productos").FontSize(20).Bold();
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(); // Id
                        columns.RelativeColumn(); // Nombre
                        columns.RelativeColumn(); // Cantidad
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("ID").Bold();
                        header.Cell().Text("Nombre").Bold();
                        header.Cell().Text("Cantidad").Bold();
                    });

                    foreach (var p in productos)
                    {
                        table.Cell().Text(p.Id.ToString());
                        table.Cell().Text(p.Nombre);
                        table.Cell().Text(p.Cantidad.ToString());
                    }
                });
            });
        });

        return document.GeneratePdf();
    }
}