using CyberPulse.Shared.Entities.Inve;
using CyberPulse.Shared.EntitiesDTO.Inve;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CyberPulse.Backend.Helpers;

public class InveReportService
{
    public static byte[] GenerarPdf(List<Segment> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE SEGMENTOS")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(5);
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Code");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Nombre");
                    cabecera.Cell().Scale(tamañoDeLetra).AlignRight().Element(EstiloCelda).AlignLeft().Text("Estado");
                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Code.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<Family> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE FAMILIAS")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(3);
                    columnas.RelativeColumn(3);
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Code");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Familia");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Segmento");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Estado");
                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Code.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Segment.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<Classe> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE CLASES")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(2);
                    columnas.RelativeColumn(3);
                    columnas.RelativeColumn(3);
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Code");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Clase");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Segmento");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Familia");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Estado");
                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Code.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Family!.Segment!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Family!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<InvProgram> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE PROGRAMAS")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn();
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Nombre");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Estado");
                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<Lot> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE LOTES")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn();
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Nombre");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Estado");
                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<ProgramLot> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE PROGRAMAS Y LOTES")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Programa");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Lote");
                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Program!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Lot!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<Course> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE CURSOS")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn();
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Código");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Curso");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Estado");
                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Code);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<CourseProgramLot> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE CURSOS Y LOTES")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(2);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(4);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Id");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Programa");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Lote");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Curso");

                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Id.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.ProgramLot!.Program!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.ProgramLot!.Lot!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Course!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<Product> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE PRODUCTOS")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(2);
                    columnas.RelativeColumn(4);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(2);
                    columnas.RelativeColumn(2);
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Unspsc");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Nombre");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Descripción");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Um");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Lote");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Clase");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Categoría");

                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Code.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Description);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.UnitMeasurement!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Lot!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Classe!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Category!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<ProductCurrentValue> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE PRECIO VIGENTE DE PRODUCTOS")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Código");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Vigencia");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Producto");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Descripción");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Lote");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("V.Mínimo");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("¨V.Máximo");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("¨Valor");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("¨I.V.A");

                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Product!.Code.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Validity!.Value.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Product!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Product!.Description);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Product.Lot!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.PriceHigh.ToString("N2"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.PriceLow.ToString("N2"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Worth.ToString("N2"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Iva!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<Budget1DTO> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }

        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(40).Column(columna =>
            {
                columna.Spacing(5);
                columna.Item().Element(ConstruirTabla);
            });
        }

        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Id");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Vigencia");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Tipo");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Rubro");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Valor");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Statu");
                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Id.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Validity!.Value.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.BudgetType!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Rubro);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Worth.ToString("N2"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }

        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }


        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<BudgetProgramIndexDTO> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE PRESUPUESTO DE PROGRAMAS")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(2);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Id");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Vigencia");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Program");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Tipo");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Rubro");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Valor");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Balance");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Estado");

                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Id.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Validity!.Value.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Program!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.BudgetType!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Budget!.Rubro);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Worth.ToString("N2"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Balance.ToString("N2"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf(List<BudgetLot> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE PRESUPUESTO DE LOTE")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(3);
                    columnas.RelativeColumn(3);
                    columnas.RelativeColumn(2);
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Id");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Programa");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Lote");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Valor");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Estado");

                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Id.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.ProgramLot!.Program!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.ProgramLot!.Lot!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Worth.ToString("N2"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] GenerarPdf2(List<BudgetCourse> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());  //tamaño personalizado horizontal
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE PRESUPUESTO DE CURSOS")
                    .FontSize(15)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });
        }
        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(10).Column(columna =>
            {
                columna.Spacing(8);
                columna.Item().Element(ConstruirTabla);
            });
        }
        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {
                var tamañoDeLetra = 0.6f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(3);
                    columnas.RelativeColumn(3);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(1);
                    columnas.RelativeColumn(1);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Código");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Curso");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Intructor");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("F.Inicio");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("F.Final");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Valor");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Estado");

                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.CourseProgramLot!.Course!.Code.ToString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.CourseProgramLot!.Course!.Name);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Instructor.FullName);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.StartDate!.Value.ToShortDateString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.EndDate!.Value.ToShortDateString());
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Worth.ToString("N2"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Statu!.Name);

                    IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingVertical(5);
                    }
                }
            });

        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }
        return document.GeneratePdf();
    }

    public static byte[] GenerarPdf(List<BudgetCourse> entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(Container =>
        {
            Container.Page(page =>
            {
                page.Margin(30);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ConstruirContenido);
                page.Footer().Element(ComposeFooter);
            });
        });

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Element(EncabezadoOriginal);
                col.Item()
                    .PaddingBottom(5)
                    .AlignCenter()
                    .Text("REPORTE DE PRESUPUESTO")
                    .FontSize(12)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);
            });

        }

        void ConstruirContenido(IContainer contenedor)
        {
            contenedor.PaddingVertical(40).Column(columna =>
            {
                columna.Spacing(5);
                columna.Item().Element(ConstruirTabla);
            });
        }

        void ConstruirTabla(IContainer contenedor)
        {
            contenedor.Table(tabla =>
            {

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn();// Nivel 1
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();// Nivel 2
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();// Nivel 3
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();// Detalle
                    columnas.RelativeColumn();
                });

                // AGRUPACIÓN NIVEL 1
                var gruposNivel1 = entity
                    .GroupBy(x => new
                    {
                        x!.BudgetLot!.BudgetProgram!.Budget!.Validity!.Value,
                        x.BudgetLot!.BudgetProgram!.Budget!.BudgetType!.Name,
                        x.BudgetLot!.BudgetProgram.Budget.Rubro,
                        Worth = x.BudgetLot.BudgetProgram.Budget.Worth,
                        StatusName = x.BudgetLot!.BudgetProgram!.Budget!.Statu!.Name
                    });

                foreach (var g1 in gruposNivel1)
                {
                    double totalNivel1 = g1.Key.Worth - g1.Sum(x => x.Worth);
                    // FILA DE TÍTULO NIVEL 1
                    tabla.Cell().ColumnSpan(11).Element(CeldaGrupo).Text($"{g1.Key.Value} | {g1.Key.Name} | Rubro: {g1.Key.Rubro} | Valor: {g1.Key.Worth:N2} | Balance: {totalNivel1:N2}");

                    // AGRUPACIÓN NIVEL 2
                    var gruposNivel2 = g1.GroupBy(x => new
                    {
                        Program = x.BudgetLot!.BudgetProgram!.Program!.Name,
                        WorthProgram = x.BudgetLot.BudgetProgram.Worth
                    });

                    foreach (var g2 in gruposNivel2)
                    {
                        double totalNivel2 = g2.Key.WorthProgram - g2.Sum(x => x.Worth);

                        tabla.Cell().ColumnSpan(11).Element(CeldaSubGrupo).Text(
                            $"      {g2.Key.Program} | Valor: {g2.Key.WorthProgram:N2} | Balance: {totalNivel2:N2}");

                        // AGRUPACIÓN NIVEL 3
                        var gruposNivel3 = g2.GroupBy(x => new
                        {
                            Lot = x.BudgetLot!.ProgramLot!.Lot!.Name,
                            WorthLot = x.BudgetLot.Worth
                        });
                        double total2 = 0;

                        foreach (var g3 in gruposNivel3)
                        {
                            double totalNivel3 = g3.Key.WorthLot - g3.Sum(x => x.Worth);
                            total2 += g3.Key.WorthLot;
                            tabla.Cell().ColumnSpan(11).Element(CeldaSubGrupo2).Text(
                                $"              {g3.Key.Lot} | Valor: {g3.Key.WorthLot:N2}");
                            double total3 = 0;
                            // DETALLE FINAL (Course)
                            foreach (var item in g3)
                            {
                                tabla.Cell().ColumnSpan(11).Element(c =>
                                {
                                    c.PaddingLeft(50)
                                    .DefaultTextStyle(x => x.FontSize(8))
                                    .BorderBottom(2).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(7)
                                    .Row(row =>
                                    {
                                        row.RelativeItem(5).Text($"{item.CourseProgramLot!.Course!.Name}");
                                        row.RelativeItem(2).AlignRight().Text($"{item.Worth:N2}");
                                    });
                                });
                                total3 += item.Worth;
                            }

                            tabla.Cell().ColumnSpan(11).Element(c =>
                            {
                                c.PaddingLeft(50)
                                .DefaultTextStyle(x => x.FontSize(8))
                                .BorderBottom(2).BorderColor(Colors.Grey.Lighten2)
                                .Padding(7)
                                .Row(row =>
                                {
                                    row.RelativeItem(5).AlignRight().Text("Sub total:");
                                    row.RelativeItem(2).AlignRight().Text($"{total3:N2}");
                                });
                            });
                        }
                        tabla.Cell().ColumnSpan(11).Element(CeldaSubGrupo2).Text(
                            $"      {g2.Key.Program} Total: {total2:N2}");

                    }
                }
            });
            // === ESTILOS ===


            static IContainer CeldaGrupo(IContainer c) =>
                c.Background(Colors.Grey.Lighten3).Padding(5).DefaultTextStyle(x => x.SemiBold().FontSize(12));

            static IContainer CeldaSubGrupo(IContainer c) =>
                c.Background(Colors.Grey.Lighten4).Padding(4).DefaultTextStyle(x => x.SemiBold().FontSize(11));

            static IContainer CeldaSubGrupo2(IContainer c) =>
                c.Background(Colors.Grey.Lighten5).Padding(3).DefaultTextStyle(x => x.SemiBold().FontSize(10));
        }

        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.ConstantItem(150).Text("Total de registros:");
                row.RelativeItem().Text(entity.Count.ToString()).Bold();
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Seccional Arauca - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }

        void EncabezadoOriginal(IContainer container)
        {
            container.Row(row =>
            {
                var tamañoDeLetra = 0.8f;
                var imagePath = $"{rutaPath}\\Images\\Reports\\Sena.jpg";
                row.RelativeItem().Column(column =>
                {
                    column.Item().Scale(tamañoDeLetra).Text("INVENTARIO DE ELEMENTOS PARA CURSOS").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath);
            });
        }

        return document.GeneratePdf();
    }
}