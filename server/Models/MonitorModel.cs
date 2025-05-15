namespace server.Models
{
    public class MonitorModel : EquipoModel
    {
        public string? Resolucion {get;set;}
        public string? Fuente {get;set;}
        public required EquipoModel Equipo {get;set;}
    }
}