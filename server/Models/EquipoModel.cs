namespace server.Models
{
    public class EquipoModel
    {
        public int NroInventario {get;set;}
        public string? NroSerie {get;set;}
        public string? Marca {get;set;}
        public string? Modelo {get;set;}
        public string? Observacion {get;set;}
    // Claves foraneas y referencias
        public int OficinaId {get;set;}
        public OficinaModel Oficina {get;set;}
    }
}