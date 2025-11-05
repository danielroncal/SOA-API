using System.ComponentModel.DataAnnotations;

namespace SOA.features.auth.dtos
{
    public class RecoverPasswordDto
    {
        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 30 caracteres.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#._-])[A-Za-z\d@$!%*?&#._-]+$",
            ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número y un símbolo."
        )]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe repetir la contraseña.")]
        public string RepeatPassword { get; set; }
    }
}
