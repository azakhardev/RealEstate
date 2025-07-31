using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{
    [Table("PropertiesInsertions")]
    public class PropertyInsertion
    {
        public int Id { get; set; }
        public int InsertionId { get; set; }
        public int PropertyId { get; set; }
        public string Value { get; set; }
    }
}
