using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context.Database;
using server.Models;
using server.Models.DTOs.Componentes;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentesController : Controller
    {
        private readonly DatabaseService _databaseService;
        private readonly IConfiguration _configuration;
        public ComponentesController(DatabaseService databaseService, IConfiguration configuration)
        {
            _databaseService = databaseService;
            _configuration = configuration;
        }

        [HttpPost()]
        [Route("crear")]
        public async Task<IActionResult> Create([FromBody] createComponente c)
        {
            var data = new ComponenteModel()
            {
                Marca = c.Marca,
                Modelo = c.Modelo,
                Cantidad = c.Cantidad,
                Tipo = c.Tipo,
                Detalle = c.Detalle
            };
            _databaseService.Componentes.Add(data);
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created, data);
        }

        [HttpPost()]
        [Route("crear-lista")]
        public async Task<IActionResult> Create([FromBody] List<createComponente> listado)
        {
            foreach (var c in listado)
            {
                var data = new ComponenteModel();
                data.Marca = c.Marca;
                data.Modelo = c.Modelo;
                data.Cantidad = c.Cantidad;
                data.Tipo = c.Tipo;
                data.Detalle = c.Detalle;
                _databaseService.Componentes.Add(data);
            }
            await _databaseService.SaveAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet()]
        [Route("listado")]
        public async Task<IActionResult> Listado()
        {
            var listado = await _databaseService.Componentes.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listado);
        }

        [HttpGet]
        [Route("listado-excel")]
        public async Task<IActionResult> ListadoExcel()
        {
            try
            {
                var componentes = await _databaseService.Componentes.ToListAsync();

                string apiUrl = (_configuration["apiUrl:excel-service"] ?? _configuration["excel-service"]) + "api/Excel/exportar-componentes";

                using var httpClient = new HttpClient();

                // Serializar los datos a JSON para enviar
                var jsonData = JsonSerializer.Serialize(componentes);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Error al generar Excel: {errorContent}");
                }

                // Obtener el stream del contenido de la respuesta
                var fileStream = await response.Content.ReadAsStreamAsync();

                // Crear un MemoryStream para poder resetear la posición
                var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                return File(
                    fileContents: memoryStream.ToArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: $"ListadoComponentes_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al generar el Excel: {ex.Message}");
            }

        }

        [HttpGet]
        [Route("modelo-excel")]
        public async Task<IActionResult> ObtenerPlantillaExcel()
        {
            try
            {
                // Configurar HttpClient (idealmente usar IHttpClientFactory)
                using var httpClient = new HttpClient();

                // URL del servicio externo (debería venir de configuración)
                string apiUrl = _configuration["apiUrl:excel-service"] + "api/Excel/nuevo-componente";

                // Realizar la petición GET al servicio externo
                var response = await httpClient.GetAsync(apiUrl);

                // Verificar si la respuesta fue exitosa
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode,
                        $"Error al obtener plantilla desde el servicio: {errorContent}");
                }

                // Obtener el stream del Excel
                var fileStream = await response.Content.ReadAsStreamAsync();

                // Crear MemoryStream para poder resetear la posición
                var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // Devolver el archivo Excel
                return File(
                    fileContents: memoryStream.ToArray(),
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: $"PlantillaComponente_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
            catch (Exception ex)
            {
                // Registrar el error (idealmente usar ILogger)
                Console.WriteLine($"Error al obtener plantilla Excel: {ex.Message}");

                return StatusCode(500, $"Error interno al obtener la plantilla: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("cargar-excel")]
        public async Task<IActionResult> cargarExcel(IFormFile file)
        {
            // Validación más robusta del archivo
            if (file == null || file.Length == 0)
                return BadRequest("No se ha proporcionado un archivo válido.");

            var validExtensions = new[] { ".xlsx", ".csv" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!validExtensions.Contains(fileExtension))
                return BadRequest("Formato de archivo no válido. Solo se aceptan .xlsx o .csv");

            try
            {
                // Configurar HttpClient (idealmente usar IHttpClientFactory)
                using var httpClient = new HttpClient();
                
                // URL del servicio externo
                string apiUrl = _configuration["apiUrl:excel-service"] + "api/Excel/importar-componente";

                // Crear contenido multipart para enviar el archivo
                using var formContent = new MultipartFormDataContent();
                using var fileStream = file.OpenReadStream();
                formContent.Add(new StreamContent(fileStream), "file", file.FileName);

                // Enviar archivo al servicio externo
                var response = await httpClient.PostAsync(apiUrl, formContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, 
                        $"Error al procesar archivo en el servicio externo: {errorContent}");
                }

                // Opción 1: Devolver directamente la respuesta del servicio externo
                var responseContent = await response.Content.ReadAsStringAsync();
                return Content(responseContent, "application/json");

                // Opción 2: Procesar la respuesta (si necesitas transformarla)
                /*
                var resultado = await response.Content.ReadFromJsonAsync<ResultadoImportacion>();
                return Ok(new {
                    Message = "Archivo procesado correctamente",
                    TotalRegistros = resultado?.TotalRegistros,
                    RegistrosError = resultado?.RegistrosConError
                });
                */
            }
            catch (Exception ex)
            {
                // Registrar el error (idealmente usar ILogger)
                Console.WriteLine($"Error al importar Excel: {ex}");
                
                return StatusCode(500, new {
                    Error = "Error interno al procesar el archivo",
                    Detalle = ex.Message
                });
            }

        }
    }
}