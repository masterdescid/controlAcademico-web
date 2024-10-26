using System.ComponentModel.DataAnnotations;

namespace controlAcademico_web.Models
{
    public class RolModel
    {


        [Display(Name = "Código")]
        public int codigoRol { get; set; }

        [Display(Name = "Nombre de rol")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(maximumLength: 64, MinimumLength = 0, ErrorMessage = "La longitud del {0} debe estar entre {2} y {1}")]
        [RegularExpression("^[a-zA-Z0-9 .,áéíóú]+$", ErrorMessage = "El campo {0} solo puede contener caracteres alfanuméricos y espacios.")]
        public string nombreRol { get; set; } = string.Empty;

        [Display(Name = "Estatus")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public byte estatus { get; set; }

    }
}
