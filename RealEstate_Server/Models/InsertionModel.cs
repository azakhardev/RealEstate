using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{
    public class InsertionModel
    {
        public Insertion Insertion { get; set; }
        public string Username { get; set; }
        public List<PropertyModel> Properties { get; set; } = new List<PropertyModel>();
        public List<Photo> Photos { get; set; } = new List<Photo>();
    }
}
