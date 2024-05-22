using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuadrifoglioAPI.Models;
using QuadrifoglioAPI.Services;
using System.Security.Claims;

namespace QuadrifoglioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationApiController : ControllerBase
    {
        private readonly GoogleMapsService _googleMapsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LocationApiController(GoogleMapsService googleMapsService, UserManager<ApplicationUser> userManager)
        {
            _googleMapsService = googleMapsService;
            _userManager = userManager;
        }

        [HttpGet("geocode")]
        public async Task<IActionResult> GetGeolocation(string address)
        {
            var geolocation = await _googleMapsService.GetGeolocationAsync(address);
            return Ok(geolocation);
        }

        [HttpGet("route")]
        public async Task<IActionResult> GetRoute(string origin, string destination)
        {
            var route = await _googleMapsService.GetRouteWithEtaAsync(origin, destination);
            return Ok(route);
        }

        [Authorize]
        [HttpPost("addAddress")]
        public async Task<IActionResult> AddAddress([FromBody] Address address)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Addresses.Add(address);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(user.Addresses);
            }

            return BadRequest("Failed to add address.");
        }
    }
}
