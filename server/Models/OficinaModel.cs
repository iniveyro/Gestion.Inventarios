namespace server.Models
{
    public class OficinaModel
    {
        public int OficinaId {get;set;}
        public string? Nombre {get;set;}
        public string? Ubicacion {get;set;}
        // Claves foraneas y referencias
        public List<EquipoModel> Equipos {get;set;}
    }
}