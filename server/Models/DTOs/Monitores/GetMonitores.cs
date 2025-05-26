namespace server.Models.DTOs.Monitores
{
    public class GetMonitores
    {
        public int NroInventario { get; set; }
        public string NroSerie {get;set;}
        public string? Marca {get;set;}
        public string? Modelo {get;set;}
        public string? Resolucion {get;set;}
        public string? Fuente {get;set;}
        public string? Oficina { get; set; }
    }
}