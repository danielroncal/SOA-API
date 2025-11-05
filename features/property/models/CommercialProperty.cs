using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA.features.properties.models
{
    [Table("commercial_properties")]
    public class CommercialProperty
    {
        [Key]
        [ForeignKey("Property")]
        public Guid Id { get; set; }

        public int Floor { get; set; } = 1;

        [Column("has_parking")]
        public bool HasParking { get; set; } = false;

        [Column("parking_spaces")]
        public int? ParkingSpaces { get; set; }

        public Property Property { get; set; } = null!;
    }
}
