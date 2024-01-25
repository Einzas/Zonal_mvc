using Microsoft.AspNetCore.Mvc;
using Zonal_mvc.Models;

namespace Zonal_mvc.Controllers
{
    public class GestionAfiliados : Controller
    {
        private readonly ApplicationDBContext _context;
        public GestionAfiliados(ApplicationDBContext context)
        {
            _context = context;
        }
        // GET: GestionUsuarios
        public ActionResult Index()
        {
            var viewModel = new AfiliadosViewModel
            {
                Afiliados = _context.AspNetAfiliados.ToList(),
                NuevoAfiliado = new AfiliadosModel()
            };

            return View(viewModel);
        }

        // GET: GestionUsuarios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Nombre, Apellido, Cedula, Direccion, Ciudad, Telefono, Genero")] AfiliadosModel nuevoAfiliado)
        {
            if (ModelState.IsValid)
            {
                // Agregar el nuevo afiliado al contexto y guardar los cambios
                _context.AspNetAfiliados.Add(nuevoAfiliado);
                _context.SaveChanges();

                // Establecer el mensaje de éxito en TempData
                TempData["message"] = "Éxito";

                // Redirigir a la acción Index para mostrar la lista actualizada
                return RedirectToAction(nameof(Index));
            }

            // Se añade el error al mensaje
            string message = "Error al crear el afiliado: " + ModelState.Values.First().Errors.First().ErrorMessage;
            TempData["message"] = message;

            // Devolver la vista con el nuevo afiliado
            return View("Index", new AfiliadosViewModel { NuevoAfiliado = nuevoAfiliado, Afiliados = _context.AspNetAfiliados.ToList() });
        }




        // GET: GestionUsuarios/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GestionUsuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GestionUsuarios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GestionUsuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
