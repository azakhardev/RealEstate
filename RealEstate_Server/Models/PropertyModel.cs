using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace RealEstate_Server.Models
{
    public class PropertyModel
    {
        public int Id { get; set; }
        public string? PropertyName { get; set; }
        public string? Value { get; set; }
    }
}
