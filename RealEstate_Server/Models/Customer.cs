using RealEstate_Server.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{    
    [Table("Customers")]
    public class Customer
    {
        public int Id { get; set; }
        [BadCustomerName(ErrorMessage ="This username is already used")]
        public string Username { get; set; }
        public string? Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        [ForeignKey("CustomerId")]
        public List<Chat>? Chats { get; set; }

        [ForeignKey("CustomerId")]
        public List<Models.Request>? Requests { get; set; }

        [ForeignKey("CustomerId")]
        public List<CustomerInsertion>? CustomerInsertions { get; set; }
    }
}
