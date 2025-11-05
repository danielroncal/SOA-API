using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SOA.features.location.models
{
    [Table("locations")]
    [Index(nameof(Slug), IsUnique = true)]
    public class Location
    {
        [Key]
        public Guid Id { get; set; }

        [Column("department_id")]
        public Guid? DepartmentId { get; set; }

        [Column("province_id")]
        public Guid? ProvinceId { get; set; }

        [Column("district_id")]
        public Guid? DistrictId { get; set; }

        [Required, MaxLength(150)]
        public string Slug { get; set; }

        [Required, MaxLength(30)]
        public string Type { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        [ForeignKey("ProvinceId")]
        public Province? Province { get; set; }

        [ForeignKey("DistrictId")]
        public District? District { get; set; }
    }
}
