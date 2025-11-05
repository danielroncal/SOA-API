using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using SOA.features.properties.models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA.features.services.models
{
    [Table("services")]
    public class Service
    {
        [Key]
        public Guid Id { get; set; }

        public string ServiceName { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public ICollection<ServiceToProperty> ServiceToProperties { get; set; } = new List<ServiceToProperty>();
    }
}
