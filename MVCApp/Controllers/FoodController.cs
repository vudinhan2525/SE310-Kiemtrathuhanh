using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;
namespace MVCApp.Controllers
{   
    public class DeleteFoodRequest
    {
        public int Id { get; set; }
    }

    public class FoodController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FoodController> _logger;
    public FoodController(ApplicationDbContext context,ILogger<FoodController> logger)
    {
        _context = context;
         _logger = logger;
    }

    [HttpGet("/Food/{id}")]
    public IActionResult Details(int id)
    {
        var food = _context.Foods.FirstOrDefault(f => f.Id == id);
        if (food == null)
        {
            return NotFound();
        }
        return View(food); 
    }

    [HttpPost]
    public IActionResult Add(Food newFood)
    {
        // Check if FoodTypeId is set, if not, assign a default value
        if (newFood.FoodTypeId <= 0) // Adjust this check based on your requirements
        {
            newFood.FoodTypeId = 1; // Default value for FoodTypeId
        }
            _context.Foods.Add(newFood); 
            _context.SaveChanges();      
            return Ok();     
        
    }
    [HttpPost("/Food/DeleteById")]
    public IActionResult DeleteById([FromBody] DeleteFoodRequest request) 
    {   
        var food = _context.Foods.FirstOrDefault(f => f.Id == request.Id);
        if (food == null)
        {
            return NotFound();
        }
        _context.Foods.Remove(food);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPost("/Food/Update")]
    public IActionResult Update([FromBody] Food updatedFood) 
    {
            var food = _context.Foods.FirstOrDefault(f => f.Id == updatedFood.Id);
            if (food == null)
            {
                return NotFound();
            }

            food.Name = updatedFood.Name;
            food.Description = updatedFood.Description;
            food.Price = updatedFood.Price;
            food.Image = updatedFood.Image;
            food.ItemLeft = updatedFood.ItemLeft;
            food.FoodTypeId = updatedFood.FoodTypeId;

            _context.SaveChanges();
            return Ok();
  
    }
    [HttpPost("/Food/GetFoodType")]
    public IActionResult GetFoodType([FromBody] FoodTypeRequest request) 
    {   
        if (string.IsNullOrEmpty(request.Type))
        {
            return BadRequest("Food type is required.");
        }
        var foodTypeMapping = new Dictionary<string, string>
        {
            { "mon-mi", "Món mì" },
            { "com", "Cơm" },
            { "mon-vat", "Món vặt" }
        };
        if (!foodTypeMapping.TryGetValue(request.Type, out var mappedFoodTypeName))
        {
            return BadRequest("Invalid food type.");
        }

        // Fetch the foods from the database
        var foods = _context.Foods
            .Where(f => f.FoodType.Name == mappedFoodTypeName)
            .ToList();

        return Ok(foods);
    }
    public class FoodTypeRequest
    {
        public string Type { get; set; }
    }
}
}
