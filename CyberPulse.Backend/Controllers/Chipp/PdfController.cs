using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace CyberPulse.Backend.Controllers.Chipp;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    [HttpGet("generate")]
    public IActionResult GeneratePdf()
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Text("Hola desde QuestPDF!");
            });
        });

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;

        return File(stream, "application/pdf", "documento.pdf");
    }
}
