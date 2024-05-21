using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QuadrifoglioAPI.DTOs;
using QuadrifoglioAPI.Models;
using QuadrifoglioAPI.Services;

namespace QuadrifoglioAPI.Controllers
{
    public class LocationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICompositeViewEngine _viewEngine;

        public LocationController(IConfiguration configuration, ICompositeViewEngine viewEngine)
        {
            _configuration = configuration;
            _viewEngine = viewEngine;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            ViewData["GoogleMapsApiKey"] = _configuration["GoogleMaps:ApiKey"];
            return View();
        }

        [HttpGet("LocationPartial")]
        public IActionResult LocationPartial()
        {
            ViewData["GoogleMapsApiKey"] = _configuration["GoogleMaps:ApiKey"];
            return PartialView("LocationPartial");
        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            var viewEngineResult = _viewEngine.FindView(ControllerContext, viewName, false);

            if (!viewEngineResult.Success)
            {
                return $"View '{viewName}' not found";
            }

            var view = viewEngineResult.View;
            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    ControllerContext,
                    view,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );

                await view.RenderAsync(viewContext);

                return sw.ToString();
            }
        }
    }



    //private readonly UserManager<ApplicationUser> _userManager;
    //private readonly UserService _userService;

    //public LocationController(UserManager<ApplicationUser> userManager, UserService userService)
    //{
    //    _userManager = userManager;
    //    _userService = userService;
    //}

    //[HttpPost("update")]
    //public async Task<IActionResult> UpdateUserLocation([FromBody] UserLocationDTO locationData)
    //{
    //    // Get the user's ID from the token or any other authentication mechanism
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //    if (userId == null)
    //        return Unauthorized(); // User is not authenticated

    //    var user = await _userManager.FindByIdAsync(userId);

    //    if (user == null)
    //        return NotFound(); // User not found

    //    // Update the user's location with the received latitude and longitude
    //    user.Latitude = locationData.Latitude;
    //    user.Longitude = locationData.Longitude;

    //    // Save the changes to the database
    //    var result = await _userManager.UpdateAsync(user);

    //    if (result.Succeeded)
    //    {
    //        // Optionally, you can also update the user's location asynchronously
    //        // This allows you to perform any additional tasks without delaying the response
    //        _ = _userService.UpdateUserLocationAsync(user);

    //        return Ok();
    //    }
    //    else
    //    {
    //        // Handle the case where updating user failed
    //        return BadRequest("Failed to update user location.");
    //    }
    //}
}
