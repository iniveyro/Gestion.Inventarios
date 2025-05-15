namespace server.Models
{
    public class UserModel
    {
        public int UserId {get;set;}
        public required string NomApe {get;set;}
        public required string Username {get;set;}
        public required string Password {get;set;}
        public string? Rol {get;set;}
    }
}