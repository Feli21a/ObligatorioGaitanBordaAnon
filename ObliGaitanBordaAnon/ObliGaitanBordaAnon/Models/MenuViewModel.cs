namespace ObliGaitanBordaAnon.Models
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string NombrePlato { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public string Categoria { get; set; }
        public string? ImagenUrl { get; set; }
    }
}
