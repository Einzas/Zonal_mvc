using Microsoft.AspNetCore.Mvc;

namespace Zonal_mvc.Controllers
{
    public class GestionUsuarios : Controller
    {
        // GET: GestionUsuarios
        public ActionResult Index()
        {
            return View();
        }

        // GET: GestionUsuarios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GestionUsuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GestionUsuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
