using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SOA.features.auth.models;
using SOA.features.location.models;
using SOA.features.properties.enums;
using SOA.features.image.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA.features.properties.models
{
    [Table("properties")]
    public class Property
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Slug { get; set; } = null!;

        [Column("property_type")]
        public PropertyType PropertyType { get; set; }

        public Currency Currency { get; set; }

        [Column("property_category")]
        public PropertyCategory PropertyCategory { get; set; }

        public double Price { get; set; }

        public string Address { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Availability { get; set; } = true;

        [Column("user_id")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string Phone { get; set; } = null!;

        [Column("year_built")]
        public int? YearBuilt { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Column("extra_info")]
        public string? ExtraInfo { get; set; }

        [Column("location_id")]
        public Guid LocationId { get; set; }
        public Location Location { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ResidentialProperty? ResidentialProperty { get; set; }
        public CommercialProperty? CommercialProperty { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<ServiceToProperty> ServicesToProperties { get; set; } = new List<ServiceToProperty>();
    }
}
