using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{
    [Table("Chats")]
    public class Chat
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }

        [ForeignKey("ChatId")]
        public List<Message> Messages { get; set; }
    }
}
