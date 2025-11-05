using System.ComponentModel.DataAnnotations;

namespace SOA.features.auth.dtos
{
    public class CheckEmailUserDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        public string Email { get; set; }
    }
}
