using Microsoft.AspNetCore.Mvc;
using Zonal_mvc.Models;

namespace Zonal_mvc.Controllers
{
    public class GestionPrestamos : Controller
    {
        private readonly ApplicationDBContext _context;

        public GestionPrestamos(ApplicationDBContext context)
        {
            _context = context;
        }
        public IActionResult GetAmortizaciones(int prestamoId)
        {
            // Obtener los datos de amortización según el ID del préstamo
            var amortizaciones = _context.AspNetAmortizaciones.Where(a => a.PrestamoID == prestamoId).ToList();

            // Puedes utilizar una vista parcial para la tabla de amortizaciones
            return PartialView("../_AmortizacionesPartial", amortizaciones);
        }
        public ActionResult Index()
        {
            var viewModel = new PrestamosViewModel
            {
                Prestamos = _context.AspNetPrestamos.ToList(),

                NuevoPrestamo = new PrestamosModel(),
                Amortizaciones = _context.AspNetAmortizaciones.ToList() // Carga las amortizaciones

            };
            viewModel.Afiliados = _context.AspNetAfiliados.ToList(); // Agrega esta línea


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("AfiliadoID, Monto, TasaInteres, Plazo, FechaInicio, FechaFin")] PrestamosModel nuevoPrestamo)
        {
            if (ModelState.IsValid)
            {

                // Establecer la fecha de fin como null (opcional)
                nuevoPrestamo.FechaFin = null;

                // Calcular la cuota mensual utilizando la fórmula de amortización francesa
                double tasaMensual = nuevoPrestamo.TasaInteres / 12 / 100; // Tasa de interés mensual
                double cuotaMensual = (nuevoPrestamo.Monto * tasaMensual) / (1 - Math.Pow(1 + tasaMensual, -nuevoPrestamo.Plazo));

                // Guardar el nuevo préstamo en la base de datos
                _context.AspNetPrestamos.Add(nuevoPrestamo);
                _context.SaveChanges();

                // Generar la tabla de amortización y guardar las amortizaciones en la base de datos
                double temp_capital_inicial = nuevoPrestamo.Monto;

                for (int cuota = 1; cuota <= nuevoPrestamo.Plazo; cuota++)
                {
                    var amortizacion = new AmortizacionesModel
                    {
                        PrestamoID = nuevoPrestamo.PrestamoID,
                        NumeroCuota = cuota,
                        FechaPago = nuevoPrestamo.FechaInicio.AddMonths(cuota).ToUniversalTime(),  // Convertir a UTC
                        MontoCapital = Math.Round(temp_capital_inicial, 2),
                        MontoInteres = Math.Round(temp_capital_inicial * tasaMensual, 2),
                        MontoCuota = Math.Round(cuotaMensual, 2),
                        MontoTotal = Math.Round(cuotaMensual - (temp_capital_inicial * tasaMensual), 2)


                    };

                    _context.AspNetAmortizaciones.Add(amortizacion);
                    temp_capital_inicial -= cuotaMensual - (temp_capital_inicial * tasaMensual);
                }

                _context.SaveChanges();

                // Establecer el mensaje de éxito en TempData
                TempData["message"] = "Éxito";

                // Redirigir a la acción Index para mostrar la lista actualizada
                return RedirectToAction(nameof(Index));
            }

            // Se añade el error al mensaje
            var errorMessages = ModelState.Values
     .SelectMany(v => v.Errors)
     .Select(e => e.ErrorMessage)
     .ToList();

            string message = "Error al crear el préstamo: " + string.Join(", ", errorMessages);
            TempData["message"] = message;

            // Recargar la lista de afiliados (puedes optimizar esto si no es necesario)
            var viewModel = new PrestamosViewModel
            {
                Prestamos = _context.AspNetPrestamos.ToList(),
                NuevoPrestamo = nuevoPrestamo,
                Afiliados = _context.AspNetAfiliados.ToList()
            };

            // Devolver la vista con el nuevo préstamo
            return View("Index", viewModel);
        }


        // GET: GestionUsuarios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }



        // GET: GestionUsuarios/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }



    }
}
