using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObliGaitanBordaAnon.Models
{
    public class ReseniaViewModel
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public int? RestauranteId { get; set; }
        public int? Puntaje { get; set; }

        [StringLength(150, ErrorMessage = "El comentario no puede tener más de 150 caracteres.")]
        public string? Comentario { get; set; }
        public DateTime? FechaResenia { get; set; }
        public string? Email { get; set; }

        [NotMapped]
        public string? DireccionRestaurante { get; set;}
    }
}
