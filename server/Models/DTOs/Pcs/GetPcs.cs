namespace server.Models.DTOs.Pcs
{
    public class GetPcs
    {
        public int NroInventario { get; set; }
        public string NroSerie { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Ram { get; set; }
        public string? TipoRam { get; set; }
        public string? Disco { get; set; }
        public string? Procesador { get; set; }
        public string? Fuente { get; set; }
        public string? Oficina { get; set; }        
    }
}