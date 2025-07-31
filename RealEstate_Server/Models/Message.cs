using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{
    [Table("Messages")]
    public class Message
    {
        public int Id { get; set; }

        public int ChatId { get; set; }
        public string Text { get; set; }
        public bool ToBroker { get; set; }
    }
}
