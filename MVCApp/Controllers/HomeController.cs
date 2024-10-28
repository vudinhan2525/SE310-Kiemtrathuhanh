using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {   
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; 

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public IActionResult Index()
        {
            var foods = _context.Foods.ToList();
            // how i can use this data to render in view
            return View(foods);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost("/register")]
        public IActionResult Register([FromBody] BodyRegister request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid user data.");
            }
            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("User registered successfully.");
        }

        public class BodyRegister
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login data.");
            }
            var user = _context.Users.SingleOrDefault(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(new { Message = "Login successful.", Name = user.Name });
        }
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
