namespace server.Models.DTOs.Equipo
{
    public class CreateMonitorModel
    {
        public int NroInventario { get; set; }
        public string? NroSerie {get;set;}
        public string? Marca {get;set;}
        public string? Modelo {get;set;}
        public string? Resolucion {get;set;}
        public string? Fuente {get;set;}
        public int oficinaId { get; set; }
    }
}