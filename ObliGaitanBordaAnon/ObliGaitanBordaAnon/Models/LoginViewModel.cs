using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObliGaitanBordaAnon.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingresa un email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ingresa una contraseña")]
        [DataType(DataType.Password)]
        public string Contrasenia { get; set; }
    }
}       
