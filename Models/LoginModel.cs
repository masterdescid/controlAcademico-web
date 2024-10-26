using System.ComponentModel.DataAnnotations;

namespace controlAcademico_web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string correo { get; set; } // Usar PascalCase para las propiedades

        [Required(ErrorMessage = "La clave es obligatoria.")]
        [MinLength(6, ErrorMessage = "La clave debe tener al menos 6 caracteres.")]
        public string clave { get; set; }
    }
}
