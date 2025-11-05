using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SOA.features.services.models;

namespace SOA.features.properties.models
{
    [Table("services_to_properties")]
    public class ServiceToProperty
    {
        [Key]
        public Guid Id { get; set; }

        [Column("property_id")]
        public Guid PropertyId { get; set; }

        [Column("service_id")]
        public Guid ServiceId { get; set; }

        public Property Property { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}
