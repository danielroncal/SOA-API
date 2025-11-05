using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOA.features.auth.models
{ 

    [Table("tokens")]
    [Index(nameof(TokenValue), IsUnique = true)]
    public class Token
    {
        [Key]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("token")]
        public int TokenValue { get; set; }

        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }

}