namespace server.Models.DTOs.Equipo
{
    public class CreateAAModel
    {
        public int NroInventario { get; set; }
        public string? NroSerie {get;set;}
        public string? Marca {get;set;}
        public string? Modelo {get;set;}
        public string? Frigorias {get;set;}
        public string? Potencia {get;set;}
        public string? Tipo {get;set;}
        public int oficinaId { get; set; }
    }
}