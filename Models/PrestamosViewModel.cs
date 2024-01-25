namespace Zonal_mvc.Models
{
    public class PrestamosViewModel
    {
        public List<PrestamosModel> Prestamos { get; set; }
        public PrestamosModel NuevoPrestamo { get; set; }
        public List<AfiliadosModel> Afiliados { get; set; }
        public List<AmortizacionesModel> Amortizaciones { get; set; } // Agrega esta línea

    }
}
