namespace server.Models.DTOs.Audiovisuales
{
    public class GetAudiovisuales
    {
        public int NroInventario { get; set; }
        public string NroSerie {get;set;}
        public string? Marca {get;set;}
        public string? Modelo {get;set;}
        public string Accesorios {get;set;}
        public string Tipo {get;set;}
        public string? Oficina { get; set; }
    }
}