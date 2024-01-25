namespace Zonal_mvc.Models
{
    public enum Genero
    {
        Masculino,
        Femenino
    }
    public class AfiliadosModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public Genero Genero { get; set; }
    }
}
