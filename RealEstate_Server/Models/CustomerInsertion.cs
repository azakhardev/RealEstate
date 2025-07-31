using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{
    [Table("CustomersInsertions")]
    public class CustomerInsertion
    {
        public int Id { get; set; }
        public int InsertionId { get; set; }
        public int CustomerId { get; set; }
    }
}
