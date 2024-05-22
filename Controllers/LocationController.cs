using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using QuadrifoglioAPI.DTOs;
using QuadrifoglioAPI.Models;
using QuadrifoglioAPI.Services;

namespace QuadrifoglioAPI.Controllers
{
    public class LocationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RestaurantService _restaurantService;

        public LocationController(IConfiguration configuration, ICompositeViewEngine viewEngine, UserManager<ApplicationUser> userManager, RestaurantService restaurantService)
        {
            _configuration = configuration;
            _viewEngine = viewEngine;
            _userManager = userManager;
            _restaurantService = restaurantService;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            ViewData["GoogleMapsApiKey"] = _configuration["GoogleMaps:ApiKey"];
            return View();
        }

        [HttpGet("LocationPartial")]
        public async Task<IActionResult> LocationPartial(string userName)
        {
            var user = await _userManager.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync();
                //.FirstOrDefaultAsync(u => u.UserName == userName);
            var restaurants = _restaurantService.GetAllRestaurants(); // Example method to retrieve all restaurants

            if (user != null)
            {
                var model = new LocationPartialViewModel
                {
                    User = user,
                    Restaurants = restaurants,
                    GoogleMapsApiKey = _configuration["GoogleMaps:ApiKey"]
                };
                return PartialView("LocationPartial", model);
            }
            else
            {
                // Create a partial view to handle no user, 
                // or make endpoint inaccessable unless authenticated
                //return PartialView("RequireSignInPartial");
                return NotFound();
            }

        }
    }
}
