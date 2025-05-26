namespace server.Models.DTOs.Aires
{
    public class GetAires
    {
        public int NroInventario { get; set; }
        public string NroSerie { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Frigorias { get; set; }
        public string? Potencia { get; set; }
        public string? Tipo { get; set; }
        public string? Oficina { get; set; }
    }
}