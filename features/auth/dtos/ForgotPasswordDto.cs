using System.ComponentModel.DataAnnotations;

namespace SOA.features.auth.dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        [MinLength(13, ErrorMessage = "El correo debe tener al menos 13 caracteres.")]
        public string Email { get; set; }
    }
}
