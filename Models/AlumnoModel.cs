using System.ComponentModel.DataAnnotations;

namespace controlAcademico_web.Models
{
    public class AlumnoModel
    {
        [Display(Name = "Código")]

        public int codigoAlumno { get; set; }
        [Display(Name = "Usuario")]
        public int codigoUsuario { get; set; }

        [Display(Name = "Nombre maestro")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 64, MinimumLength = 0, ErrorMessage = "La longitud del {0} debe estar entre {2} y {1}")]
        [RegularExpression("^[a-zA-Z0-9 .,áéíóú]+$", ErrorMessage = "El campo {0} solo puede contener caracteres alfanuméricos y espacios.")]
        public string nombreAlumno { get; set; } = null!;

        [Display(Name = "Nombre maestro")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 64, MinimumLength = 0, ErrorMessage = "La longitud del {0} debe estar entre {2} y {1}")]
        [RegularExpression("^[a-zA-Z0-9 .,áéíóú]+$", ErrorMessage = "El campo {0} solo puede contener caracteres alfanuméricos y espacios.")]
        public string grado { get; set; } = null!;

        [Display(Name = "Estatus")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public byte estatus { get; set; }

    }
}
