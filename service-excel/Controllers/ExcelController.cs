using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using service_excel.Models;
using service_excel.Services;

namespace service_excel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExcelController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ExcelController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("nuevo-componente")] //La idea de este endopint es que devuelva un excel en blanco con la estructura para un determinado modelo
        public IActionResult NuevoExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                // Añadir una hoja
                var worksheet = workbook.Worksheets.Add("Hoja1");

                // Encabezado de tabla
                worksheet.Cell("A1").Value = "Marca";
                worksheet.Cell("B1").Value = "Modelo";
                worksheet.Cell("C1").Value = "Tipo";
                worksheet.Cell("D1").Value = "Cantidad";
                worksheet.Cell("E1").Value = "Detalle";

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    return File(
                        fileContents: stream.ToArray(),
                        contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileDownloadName: "PlantillaEnBlanco.xlsx"
                    );
                }
            }
        }

        [HttpPost]
        [Route("importar-componente")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            try
            {
                var componentes = ExcelComponentes.Import(file);
                // Enviar a la API externa
                string apiUrl = (_configuration["apiUrl:url"] ?? _configuration["apiUrl"]) + "api/Componentes/crear-lista";

                using var httpClient = new HttpClient();

                var response = await httpClient.PostAsJsonAsync(apiUrl, componentes.Result);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Error al enviar datos a la API: {errorContent}");
                }

                return Ok(new
                {
                    Message = "Datos importados y enviados correctamente",
                    ApiResponse = await response.Content.ReadAsStringAsync()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("exportar-componentes")]
        public IActionResult ExportarExcel([FromBody] List<ComponenteDTO> componentes)
        {
            try
            {
                // Validar datos de entrada
                if (componentes == null || !componentes.Any())
                {
                    return BadRequest("No se proporcionaron datos para generar el Excel");
                }

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Componentes");

                // Escribir cabeceras
                worksheet.Cell(1, 1).Value = "Marca";
                worksheet.Cell(1, 2).Value = "Modelo";
                worksheet.Cell(1, 3).Value = "Detalle";
                worksheet.Cell(1, 4).Value = "Tipo";
                worksheet.Cell(1, 5).Value = "Cantidad";

                // Escribir datos
                int row = 2;
                foreach (var componente in componentes)
                {
                    worksheet.Cell(row, 1).Value = componente.Marca;
                    worksheet.Cell(row, 2).Value = componente.Modelo;
                    worksheet.Cell(row, 3).Value = componente.Detalle;
                    worksheet.Cell(row, 4).Value = componente.Tipo;
                    worksheet.Cell(row, 5).Value = componente.Cantidad;
                    row++;
                }

                // Formato de tabla
                var range = worksheet.Range(1, 1, row - 1, 5);
                var table = range.CreateTable();
                
                // Ajustar columnas al contenido
                worksheet.Columns().AdjustToContents();

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;

                return File(
                    fileContents: memoryStream.ToArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: $"ListadoComponentes_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al generar el archivo Excel: {ex.Message}");
            }
        }
    }
}