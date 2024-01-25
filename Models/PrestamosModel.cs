namespace Zonal_mvc.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PrestamosModel
{
    [Key]
    public int PrestamoID { get; set; }
    [ForeignKey("AfiliadoID")]
    public int AfiliadoID { get; set; }
    public AfiliadosModel? Afiliado { get; set; }  // Propiedad de navegación

    public double Monto { get; set; }
    public double TasaInteres { get; set; }
    public int Plazo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public double CuotaMensual { get; set; }
    public double CapitalInicial { get; set; }
    public ICollection<AmortizacionesModel>? Amortizaciones { get; set; }




}
