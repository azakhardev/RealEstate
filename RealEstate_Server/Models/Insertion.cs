using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace RealEstate_Server.Models
{
    [Table("Insertions")]
    public class Insertion
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Disabled { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        [RegularExpression(@"^[1-9][0-9]*\+(kk|1)$", ErrorMessage = "Wrong format of Type. Must cointain number then + and kk or 1")]
        public string Type { get; set; }
        public double Area { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Photo {  get; set; }

        [ForeignKey("InsertionId")]
        public List<Request>? Requests { get; set; }

        [ForeignKey("InsertionId")]
        public List<PropertyInsertion>? PropertyInsertions { get; set; }

        [ForeignKey("InsertionId")]
        public List<Photo>? Photos { get; set; }

        [ForeignKey("InsertionId")]
        public List<CustomerInsertion>? CustomerInsertions { get; set; }
    }
}
