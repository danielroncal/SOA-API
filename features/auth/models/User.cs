using Microsoft.EntityFrameworkCore; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SOA.features.properties.models;

namespace SOA.features.auth.models
{
    [Table("users")] 
    [Index(nameof(Email), IsUnique = true)] 
    [Index(nameof(Phone), IsUnique = true)] 
    public class User
    {

        [Key] 
        public Guid Id { get; set; } 

        [Required] 
        public string Name { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Email { get; set; }

        public bool Confirmed { get; set; } 

        [Column("birth_date")]
        public DateTime? BirthDate { get; set; }

        public string Password { get; set; }
        public string Phone { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } 

        public Token? Token { get; set; }

        
        public ICollection<Property> Properties { get; set; } = new List<Property>();

        //public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
