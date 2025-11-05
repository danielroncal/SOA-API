using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SOA.features.auth.dtos
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 30 caracteres.")]
        public string Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El apellido debe tener entre 3 y 30 caracteres.")]
        public string Lastname { get; set; }

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

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [StringLength(9, MinimumLength = 9, ErrorMessage = "El número de teléfono debe tener exactamente 9 dígitos.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El número de teléfono debe tener solo dígitos.")]
        public string? Phone { get; set; }
    }
}
