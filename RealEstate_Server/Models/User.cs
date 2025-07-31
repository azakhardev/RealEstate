using RealEstate_Server.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }

        [StringLength(255, ErrorMessage = "Username cant be longer than 255 characters")]
        [MinLength(3, ErrorMessage = "Username must be longer than 2 characters")]
        [BadUserName(ErrorMessage ="Username is already used")]
        public string Username { get; set; }

        public string? Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
        
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Role { get; set; }

        public string? Photo { get; set; }

        [ForeignKey("UserId")]
        public List<Insertion>? Insertions { get; set; }

        [ForeignKey("UserId")]
        public List<Chat>? Chats { get; set; }
    }
}
