using CyberPulse.Shared.Entities.Chipp;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CyberPulse.Backend.Helpers;

public class ChipReporteService
{
    public static byte[] GenerarPdf(List<Chip> entity, string rutaPath)
    {

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(container =>
        {

            container
                .Page(page =>
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
                    column.Item().Scale(tamañoDeLetra).Text("CARACTERIZACIÓN DE FPI").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).Text("SENA Regional Arauca");
                    column.Item().Scale(tamañoDeLetra).Text("Carrera 20 No. 28-163");
                    column.Item().Scale(tamañoDeLetra).Text("Nit: 899.999.034-1");
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath); // Puedes reemplazar esto con el logo de tu empresa

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
                var tamañoDeLetraDesignation = 0.8f;

                tabla.ColumnsDefinition(columnas =>
                {
                    columnas.RelativeColumn();
                    columnas.RelativeColumn(2);
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn();
                    columnas.RelativeColumn(4);
                });
                tabla.Header(cabecera =>
                {
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Ficha No");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Instructor");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("F. Inicio");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("F. Final");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("F. alerta");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Codigo");
                    cabecera.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text("Programa");

                    static IContainer EstiloCelda(IContainer contenedor)
                    {
                        return contenedor.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                foreach (var item in entity)
                {
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.ChipNo);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.Instructor.FullName);
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.StartDate.ToString("dd/MM/yyyy"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.EndDate.ToString("dd/MM/yyyy"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.AlertDate.ToString("dd/MM/yyyy"));
                    tabla.Cell().Scale(tamañoDeLetra).Element(EstiloCelda).AlignLeft().Text(item.ChipProgram.Code);
                    tabla.Cell().Scale(tamañoDeLetraDesignation).Element(EstiloCelda).AlignLeft().Text(item.ChipProgram.Designation.ToLower());


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
    public static byte[] ChipPdf(Chip entity, string rutaPath)
    {

        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(container =>
        {

            container
                .Page(page =>
                {
                    page.Margin(30);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
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
                    column.Item().Scale(tamañoDeLetra).Text("CARACTERIZACIÓN DE CURSOS FPI").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath); // Puedes reemplazar esto con el logo de tu empresa

            });
        }

        void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                var tamañoDeLetra = 0.8f;
                var tamañoDeLetraTitle = 1f;

                column.Spacing(10); // Espacio entre secciones

                // Sección de Información General
                column.Item().Scale(tamañoDeLetra).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Información General").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Ficha No:");
                        row.RelativeItem().Text(entity.ChipNo);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Programa: ");
                        row.RelativeItem().Text(entity.ChipProgram.Designation);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Compañía:");
                        row.RelativeItem().Text(entity.Company);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Instructor: ");
                        row.RelativeItem().Text(entity.Instructor.FullName);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Estado");
                        row.RelativeItem().Text(entity.Statu.Name);
                    });
                });

                // Sección de Fechas
                column.Item().Scale(tamañoDeLetra).PaddingTop(10).BorderBottom(1).BorderBottom(Colors.Grey.Medium).PaddingBottom(1).Text("Fechas").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Fecha de Inicio:");
                        row.RelativeItem().Text(entity.StartDate.ToShortDateString());
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Fecha de Fin:");
                        row.RelativeItem().Text(entity.EndDate.ToShortDateString());
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Fecha de Alerta:");
                        row.RelativeItem().Text(entity.AlertDate.ToShortDateString());
                    });
                });

                // Sección de Programa y Ubicación
                column.Item().Scale(tamañoDeLetraTitle).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Ubicación").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Municipio:");
                        row.RelativeItem().Text(entity.Neighborhood.City!.Name);
                        if (entity.NeighborhoodId.ToString().Substring(5) != "000")
                        {
                            row.RelativeItem().Text(entity.Neighborhood.Name);
                        }
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Programa  a la medida: ");
                        row.RelativeItem().Text(entity.TrainingProgram!.Name);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Formación:");
                        row.RelativeItem().Text(entity.TypeOfTraining.Name);
                    });
                });

                // Sección de Justificación y Aprendices
                column.Item().Scale(tamañoDeLetraTitle).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Justificación y Aprendices").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Aprendices:");
                        row.RelativeItem().Text(entity.Apprentices.ToString());
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Justificación:");
                        row.RelativeItem().Text(entity.Justification).FontSize(10); // Texto más pequeño si es largo
                    });
                });


                //// Sección de Estado
                //column.Item().Scale(tamañoDeLetraTitle).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Estado").Bold();
                //column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                //{
                //    subColumn.Item().Row(row =>
                //    {
                //        row.ConstantItem(120).Text("Estado:");
                //        row.RelativeItem().Text(entity.Statu.Name);
                //    });
                //});

                // Sección de Horario Semanal
                column.Item().Scale(tamañoDeLetraTitle).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Horario Semanal").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                        string monday = lmValidar(entity.Monday);
                        string tuesday = lmValidar(entity.Tuesday);
                        string wednesday = lmValidar(entity.Wednesday);
                        string tursday = lmValidar(entity.Tursday);
                        string friday = lmValidar(entity.Friday);
                        string saturday = lmValidar(entity.Saturday);
                        string sunday = lmValidar(entity.Sunday);
                        table.Header(header =>
                        {
                            if (monday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Lunes").Bold();
                            }
                            if (tuesday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Martes").Bold();
                            }
                            if (wednesday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Miércoles").Bold();
                            }
                            if (tursday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Jueves").Bold();
                            }
                            if (friday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Viernes").Bold();
                            }
                            if (saturday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Sábado").Bold();
                            }
                            if (sunday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Domingo").Bold();
                            }
                        });

                        if (monday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(monday);
                        }
                        if (tuesday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(tuesday);
                        }
                        if (wednesday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(wednesday);
                        }
                        if (tursday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(tursday);
                        }
                        if (friday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(friday);
                        }
                        if (saturday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(saturday);
                        }
                        if (sunday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(sunday);
                        }

                        string lmValidar(string day)
                        {
                            string result = "";

                            if (day.Substring(0, 11) != "00:00-00:00")
                            {
                                result = day.Substring(0, 11);
                            }

                            if (day.Substring(12) != "00:00-00:00")
                            {
                                if (result != "")
                                {
                                    result = $"{result}\n{day.Substring(12)}";
                                }
                                else
                                {
                                    result = day.Substring(12);
                                }
                            }

                            return result;
                        }


                    });
                });

                // Sección de Horario Semanal
                column.Item().Scale(tamañoDeLetraTitle).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Tipo de población al cual pertenece").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    if (entity.ChipPoblations != null && entity.ChipPoblations.Any())
                    {
                        subColumn.Item().Scale(tamañoDeLetraTitle).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40); // Consecutivo
                                columns.RelativeColumn(3);  // Tipo de Población
                                columns.RelativeColumn(1);  // Cantidad
                            });

                            table.Header(header =>
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignCenter().AlignMiddle().Text("Item").Bold();
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignMiddle().Text("Tipo de Población").Bold();
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignCenter().AlignMiddle().Text("Cantidad").Bold();
                            });


                            int consecutivo = 1;
                            foreach (var poblation in entity.ChipPoblations)
                            {
                                table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignCenter().AlignMiddle().Text(consecutivo++.ToString());
                                //table.Cell().Border(1).Padding(5).AlignMiddle().Text(poblation.TypeOfPoblation?.Name ?? "N/A"); // Usa el nombre del tipo de población
                                table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignMiddle().Text(poblation.TypePoblation.Name); // Usa el nombre del tipo de población
                                table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignCenter().AlignMiddle().Text(poblation.Quantity.ToString());
                            }

                        });
                    }
                    else
                    {
                        subColumn.Item().Text("No se ha registrado información de población para este chip.").Italic();
                    }
                });

                // Si hay más espacio, puedes añadir una sección para comentarios o firmas
                column.Item().PaddingTop(20).Text("_________________________").FontSize(12).Bold();
                column.Item().Text("Firma del Responsable").FontSize(10);
            });
        }
        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Dirección General/Regional/Centro - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();

                });
            });
        }
        return document.GeneratePdf();
    }
    public static byte[] ChipGenePdf(Chip entity, string rutaPath)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(container =>
        {

            container
                .Page(page =>
                {
                    page.Margin(30);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
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
                    column.Item().Scale(tamañoDeLetra).Text("CARACTERIZACIÓN DE CURSOS FPI").FontSize(15).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Scale(tamañoDeLetra).PaddingTop(1).BorderColor(Colors.Grey.Medium).Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
                });

                row.ConstantItem(100).Height(2, Unit.Centimetre).Image(imagePath); // Puedes reemplazar esto con el logo de tu empresa

            });
        }

        void ComposeContent(IContainer container)
        {
            container.Column(column =>
            {
                var tamañoDeLetra = 0.8f;
                var tamañoDeLetraTitle = 1f;

                column.Spacing(10); // Espacio entre secciones

                // Sección de Información General
                column.Item().Scale(tamañoDeLetra).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Información General").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Ficha No:");
                        row.RelativeItem().Text(entity.ChipNo);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Programa: ");
                        row.RelativeItem().Text(entity.ChipProgram.Designation);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Compañía:");
                        row.RelativeItem().Text(entity.Company);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Instructor: ");
                        row.RelativeItem().Text(entity.Instructor.FullName);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Estado");
                        row.RelativeItem().Text(entity.Statu.Name);
                    });
                });

                // Sección de Fechas
                column.Item().Scale(tamañoDeLetra).PaddingTop(10).BorderBottom(1).BorderBottom(Colors.Grey.Medium).PaddingBottom(1).Text("Fechas").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Fecha de Inicio:");
                        row.RelativeItem().Text(entity.StartDate.ToShortDateString());
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Fecha de Fin:");
                        row.RelativeItem().Text(entity.EndDate.ToShortDateString());
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Fecha de Alerta:");
                        row.RelativeItem().Text(entity.AlertDate.ToShortDateString());
                    });
                });

                // Sección de Programa y Ubicación
                column.Item().Scale(tamañoDeLetraTitle).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Ubicación").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Municipio:");
                        row.RelativeItem().Text(entity.Neighborhood.City!.Name);
                        if (entity.NeighborhoodId.ToString().Substring(5) != "000")
                        {
                            row.RelativeItem().Text(entity.Neighborhood.Name);
                        }
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Programa  a la medida: ");
                        row.RelativeItem().Text(entity.TrainingProgram!.Name);
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Formación:");
                        row.RelativeItem().Text(entity.TypeOfTraining.Name);
                    });
                });

                // Sección de Justificación y Aprendices
                column.Item().Scale(tamañoDeLetraTitle).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Justificación y Aprendices").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Aprendices:");
                        row.RelativeItem().Text(entity.Apprentices.ToString());
                    });
                    subColumn.Item().Row(row =>
                    {
                        row.ConstantItem(150).Text("Justificación:");
                        row.RelativeItem().Text(entity.Justification).FontSize(10); // Texto más pequeño si es largo
                    });
                });

                // Sección de Horario Semanal
                column.Item().Scale(tamañoDeLetraTitle).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Horario Semanal").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    subColumn.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                        string monday = lmValidar(entity.Monday);
                        string tuesday = lmValidar(entity.Tuesday);
                        string wednesday = lmValidar(entity.Wednesday);
                        string tursday = lmValidar(entity.Tursday);
                        string friday = lmValidar(entity.Friday);
                        string saturday = lmValidar(entity.Saturday);
                        string sunday = lmValidar(entity.Sunday);
                        table.Header(header =>
                        {
                            if (monday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Lunes").Bold();
                            }
                            if (tuesday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Martes").Bold();
                            }
                            if (wednesday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Miércoles").Bold();
                            }
                            if (tursday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Jueves").Bold();
                            }
                            if (friday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Viernes").Bold();
                            }
                            if (saturday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Sábado").Bold();
                            }
                            if (sunday != "")
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text("Domingo").Bold();
                            }
                        });

                        if (monday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(monday);
                        }
                        if (tuesday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(tuesday);
                        }
                        if (wednesday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(wednesday);
                        }
                        if (tursday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(tursday);
                        }
                        if (friday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(friday);
                        }
                        if (saturday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(saturday);
                        }
                        if (sunday != "")
                        {
                            table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(1).AlignCenter().AlignMiddle().Text(sunday);
                        }

                        string lmValidar(string day)
                        {
                            string result = "";

                            if (day.Substring(0, 11) != "00:00-00:00")
                            {
                                result = day.Substring(0, 11);
                            }

                            if (day.Substring(12) != "00:00-00:00")
                            {
                                if (result != "")
                                {
                                    result = $"{result}\n{day.Substring(12)}";
                                }
                                else
                                {
                                    result = day.Substring(12);
                                }
                            }

                            return result;
                        }


                    });
                });

                // Sección de Horario Semanal
                column.Item().Scale(tamañoDeLetraTitle).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium).PaddingBottom(1).Text("Tipo de población al cual pertenece").Bold();
                column.Item().Scale(tamañoDeLetra).PaddingLeft(10).Column(subColumn =>
                {
                    if (entity.ChipPoblations != null && entity.ChipPoblations.Any())
                    {
                        subColumn.Item().Scale(tamañoDeLetraTitle).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40); // Consecutivo
                                columns.RelativeColumn(3);  // Tipo de Población
                                columns.RelativeColumn(1);  // Cantidad
                            });

                            table.Header(header =>
                            {
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignCenter().AlignMiddle().Text("Item").Bold();
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignMiddle().Text("Tipo de Población").Bold();
                                header.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignCenter().AlignMiddle().Text("Cantidad").Bold();
                            });


                            int consecutivo = 1;
                            foreach (var poblation in entity.ChipPoblations)
                            {
                                table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignCenter().AlignMiddle().Text(consecutivo++.ToString());
                                //table.Cell().Border(1).Padding(5).AlignMiddle().Text(poblation.TypeOfPoblation?.Name ?? "N/A"); // Usa el nombre del tipo de población
                                table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignMiddle().Text(poblation.TypePoblation.Name); // Usa el nombre del tipo de población
                                table.Cell().Border(0).BorderColor(Colors.Grey.Medium).Padding(5).AlignCenter().AlignMiddle().Text(poblation.Quantity.ToString());
                            }

                        });
                    }
                    else
                    {
                        subColumn.Item().Text("No se ha registrado información de población para este chip.").Italic();
                    }
                });

                // Si hay más espacio, puedes añadir una sección para comentarios o firmas
                column.Item().PaddingTop(20).Text("_________________________").FontSize(12).Bold();
                column.Item().Text("Firma del Responsable").FontSize(10);
            });
        }


        void ComposeFooter(IContainer container)
        {
            var tamañoDeLetraTitle = 0.8f;

            container.Row(row =>
            {
                row.RelativeItem().Scale(tamañoDeLetraTitle).Text(text =>
                {
                    text.Span("Dirección General/Regional/Centro - ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();

                });
            });
        }
        return document.GeneratePdf();
    }
}
