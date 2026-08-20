using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Touchliga.Application.Common.Interfaces;

namespace Touchliga.Infrastructure.Reportes;

public sealed class QuestPdfReporteAuditoriaService : IReporteAuditoriaPdfService
{
    public byte[] Generar(
        string titulo,
        string subtitulo,
        List<string> columnas,
        List<(string participante, List<string> valores)> filas)
    {
        var documento = Document.Create(contenedor =>
        {
            contenedor.Page(pagina =>
            {
                // Orientación horizontal -- con muchos partidos como
                // columnas, se necesita el ancho extra.
                pagina.Size(PageSizes.A4.Landscape());
                pagina.Margin(24);
                pagina.DefaultTextStyle(x => x.FontSize(8));

                pagina.Header().Column(encabezado =>
                {
                    encabezado.Item().Text(titulo).FontSize(16).Bold();
                    encabezado.Item().Text(subtitulo).FontSize(10).FontColor(Colors.Grey.Darken1);
                    encabezado.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                });

                pagina.Content().PaddingTop(10).Table(tabla =>
                {
                    tabla.ColumnsDefinition(definicion =>
                    {
                        definicion.ConstantColumn(110); // Nombre del participante
                        for (var i = 0; i < columnas.Count; i++)
                            definicion.RelativeColumn();
                    });

                    tabla.Header(encabezadoTabla =>
                    {
                        void Celda(string texto)
                        {
                            encabezadoTabla.Cell().Background(Colors.Grey.Lighten3)
                                .Padding(4)
                                .Text(texto).Bold().FontSize(7);
                        }

                        Celda("Participante");
                        foreach (var columna in columnas)
                            Celda(columna);
                    });

                    foreach (var fila in filas)
                    {
                        tabla.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten2)
                            .Padding(4).Text(fila.participante).FontSize(7);

                        foreach (var valor in fila.valores)
                        {
                            tabla.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten2)
                                .Padding(4).AlignCenter()
                                .Text(string.IsNullOrWhiteSpace(valor) ? "-" : valor).FontSize(7);
                        }
                    }
                });

                pagina.Footer().AlignCenter().Text(texto =>
                {
                    texto.Span("Generado el ");
                    texto.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).SemiBold();
                });
            });
        });

        return documento.GeneratePdf();
    }
}
