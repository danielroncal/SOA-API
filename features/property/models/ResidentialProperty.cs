using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA.features.properties.models
{
    [Table("residential_properties")]
    public class ResidentialProperty
    {
        [Key]
        [ForeignKey("Property")]
        public Guid Id { get; set; }

        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public double Area { get; set; }
        public bool Furnished { get; set; } = false;

        [Column("has_terrace")]
        public bool HasTerrace { get; set; } = false;

        public Property Property { get; set; } = null!;
    }
}
