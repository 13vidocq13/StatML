using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StatML.Models;

namespace StatML.Controllers
{
    public class PagesController : Controller
    {
        readonly AppDbContext _dbContext;
        public PagesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: IndexController
        public IActionResult Index()
        {
            var data = new Player { Id = 1, PName = "test123", HorizontalPosition = 1, VerticalPosition = 1 };
            _dbContext.Players.Add(data);
            _dbContext.SaveChanges();

            return View(_dbContext.Players.ToList());
        }

        // GET: PagesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PagesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PagesController/Create
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

        // GET: PagesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PagesController/Edit/5
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

        // GET: PagesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PagesController/Delete/5
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
