using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObliGaitanBordaAnon.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}       
