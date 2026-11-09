using Microsoft.AspNetCore.Identity;

namespace StatML.Models
{
    public class Login
    {
        public int Id { get; set; } 
        public int PlayerId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        public Player? PlayerEntity { get; set; }
        public Role? RoleEntity { get; set; }
    }
}
