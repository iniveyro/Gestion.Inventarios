namespace server.Models.DTOs.Impresoras
{
    public class GetImpresora
    {
        public int NroInventario { get; set; }
        public string NroSerie { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string TonnerModelo { get; set; }
        public string Tipo { get; set; }
        public string Consumible { get; set; }
        public string Oficina { get; set; }
    }
}