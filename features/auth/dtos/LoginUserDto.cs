using System.ComponentModel.DataAnnotations;

namespace SOA.features.auth.dtos
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        [MinLength(13, ErrorMessage = "El correo debe tener al menos 13 caracteres.")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 30 caracteres.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#._-])[A-Za-z\d@$!%*?&#._-]+$",
            ErrorMessage = "La contraseña debe incluir al menos una minúscula, una mayúscula, un número y un carácter especial."
        )]
        public string Password { get; set; }
    }
}
