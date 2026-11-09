using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Controllers
{
    public class IndexController : Controller
    {
        readonly AppDbContext _dbContext;
        public IndexController(AppDbContext dbContext)
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

        // GET: IndexController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IndexController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IndexController/Create
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

        // GET: IndexController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IndexController/Edit/5
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

        // GET: IndexController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IndexController/Delete/5
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
