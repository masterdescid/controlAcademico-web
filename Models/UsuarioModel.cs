using System.ComponentModel.DataAnnotations;

namespace controlAcademico_web.Models
{
    public class UsuarioModel
    {

        [Display(Name = "Código")]
        public int codigoUsuario { get; set; }
        [Display(Name = "Rol")]
        public int codigoRol { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(64, MinimumLength = 5, ErrorMessage = "La longitud del {0} debe estar entre {2} y {1} caracteres.")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser una dirección de correo válida.")]
        public string correo { get; set; } = string.Empty;

        [Display(Name = "Clave")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "La longitud del {0} debe estar entre {2} y {1} caracteres.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
            ErrorMessage = "El campo {0} debe tener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public string? clave { get; set; }

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.Date, ErrorMessage = "El campo {0} debe ser una fecha válida.")]
        public DateTime? fechaRegistro { get; set; }

        [Display(Name = "Estatus")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public byte estatus { get; set; }

    }
}
