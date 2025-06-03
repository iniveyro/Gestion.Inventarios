using ClosedXML.Excel;
using service_excel.Models;

namespace service_excel.Services
{
    public class ExcelComponentes
    {
        public static async Task<List<ComponenteDTO>> Import(IFormFile file)
        {
            // Procesar Excel
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var componentes = new List<ComponenteDTO>();

            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1);
                var headers = worksheet.Row(1).Cells().Select(c => c.Value.ToString()).ToList();

                foreach (var row in worksheet.RowsUsed().Skip(1)) // Saltar la fila de encabezados
                {

                    var componente = new ComponenteDTO
                    {
                        Marca = row.Cell(1).Value.ToString() ?? string.Empty,
                        Modelo = row.Cell(2).Value.ToString() ?? string.Empty,
                        Tipo = row.Cell(3).Value.ToString() ?? string.Empty,
                        Cantidad = int.TryParse(row.Cell(4).Value.ToString(), out int cantidad) ? cantidad : 0,
                        Detalle = row.Cell(5).Value.ToString() ?? string.Empty,
                    };

                    componentes.Add(componente);
                }
            }
            return componentes;
        }
    }
}

