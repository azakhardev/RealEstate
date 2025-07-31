using Org.BouncyCastle.Bcpg.OpenPgp;

namespace RealEstate_Server.Models
{
    public class Filter
    {
        public int? StartPrice { get; set; }
        public int? EndPrice { get; set; }

        public string? Location { get; set; }
        public string? Type { get; set; }

        public float? StartArea { get; set; }
        public float? EndArea { get; set; }
    }
}
