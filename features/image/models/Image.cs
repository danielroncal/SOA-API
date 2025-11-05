using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SOA.features.properties.enums;
using SOA.features.properties.models;

namespace SOA.features.image.models
{
    [Table("images")]
    public class Image
    {
        [Key]
        public Guid Id { get; set; }

        [Column("property_id")]
        public Guid PropertyId { get; set; }

        [Column("public_id")]
        public string PublicId { get; set; } = null!;

        public string Url { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ImageType Type { get; set; }

        public Property Property { get; set; } = null!;
    }
}
