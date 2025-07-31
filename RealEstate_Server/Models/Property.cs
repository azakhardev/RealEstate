using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{
    [Table("Properties")]
    public class Property
    {
        public int Id { get; set; }
        [Column("Property")]
        public string PropertyName { get; set; }

        [ForeignKey("PropertyId")]
        public List<PropertyInsertion>? PropertiesInsertions { get; set; }
    }
}
