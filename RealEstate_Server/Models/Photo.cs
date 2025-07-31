using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate_Server.Models
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public int InsertionId { get; set; }
        public bool Main { get; set; }
        public string Path { get; set; }
    }
}
