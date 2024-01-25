using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zonal_mvc.Models
{
    public class AmortizacionesModel
    {
        [Key]
        public int AmortizacionID { get; set; }
        [ForeignKey("PrestamoID")]
        public int PrestamoID { get; set; }
        public PrestamosModel Prestamo { get; set; }  // Propiedad de navegación
        public int NumeroCuota { get; set; }
        public DateTime FechaPago { get; set; }
        public double MontoCapital { get; set; }
        public double MontoInteres { get; set; }
        public double MontoCuota { get; set; }
        public double MontoTotal { get; set; }



    }
}
