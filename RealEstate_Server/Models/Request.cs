using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{
    [Table("Requests")]
    public class Request
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int InsertionId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
    }
}
