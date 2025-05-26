namespace server.Models.DTOs.User
{
    public class GetUser
    {
        public int UserId {get;set;}
        public string NomApe {get;set;}
        public string Username {get;set;}
        public bool EsAdmin {get;set;}
    }
}