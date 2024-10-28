using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class StatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action method to handle "/stat"
        [HttpGet("/stat")]
        public IActionResult Index()
        {
            var foods = _context.Foods.ToList();

            // Pass data to the view
            return View(foods);
        }
    }
}
