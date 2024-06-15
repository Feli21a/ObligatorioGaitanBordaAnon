using System.ComponentModel.DataAnnotations;

namespace ObliGaitanBordaAnon.Models
{
    public class ReseniaViewModel
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public int? RestauranteId { get; set; }
        public int? Puntaje { get; set; }
        [StringLength(200, ErrorMessage = "El comentario no puede tener más de 200 caracteres.")]
        public string? Comentario { get; set; }
        public DateTime? FechaResenia { get; set; }
        public string? Email { get; set; }
    }
}
