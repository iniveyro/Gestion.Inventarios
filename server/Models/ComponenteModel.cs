namespace server.Models
{
    public class ComponenteModel
    {
        public int IdComp { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Detalle { get; set; }
        public string? Tipo { get; set; }
        public int Cantidad { get; set; }
    }
}