using System.ComponentModel.DataAnnotations;

namespace SOA.features.auth.dtos
{
    public class CompleteAccountDto
    {
        [Required]
        [RegularExpression(@"^[0-9a-fA-F]{8}\b-[0-9a-fA-F]{4}\b-[1-5][0-9a-fA-F]{3}\b-[89abAB][0-9a-fA-F]{3}\b-[0-9a-fA-F]{12}$",
            ErrorMessage = "El ID debe ser un UUID válido.")]
        public string Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 30 caracteres.")]
        public string Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El apellido debe tener entre 3 y 30 caracteres.")]
        public string Lastname { get; set; }

        [StringLength(9, MinimumLength = 9, ErrorMessage = "El número telefónico debe tener exactamente 9 dígitos.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El número telefónico solo debe contener dígitos.")]
        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
    }
}
