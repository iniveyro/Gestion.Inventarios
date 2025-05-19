namespace server.Models
{
    public class UserModel
    {
        public int UserId {get;set;}
        public string NomApe {get;set;}
        public string Username {get;set;}
        public string Password {get;set;}
        public bool EsAdmin {get;set;}
    }
}