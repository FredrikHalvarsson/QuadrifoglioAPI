using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuadrifoglioAPI.Services;

namespace QuadrifoglioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationApiController : ControllerBase
    {
        private readonly GoogleMapsService _googleMapsService;
        private readonly IConfiguration _configuration;

        public LocationApiController(GoogleMapsService googleMapsService, IConfiguration configuration)
        {
            _googleMapsService = googleMapsService;
            _configuration = configuration;
        }

        [HttpGet("geocode")]
        public async Task<IActionResult> GetGeolocation(string address)
        {
            // Check if the address is in the lat,lng format
            if (address.Contains(","))
            {
                var latLng = address.Split(',');
                var geolocation = await _googleMapsService.GetGeolocationByLatLngAsync(latLng[0], latLng[1]);
                return Ok(geolocation);
            }
            else
            {
                var geolocation = await _googleMapsService.GetGeolocationAsync(address);
                return Ok(geolocation);
            }
        }

        [HttpGet("route")]
        public async Task<IActionResult> GetRoute(string origin, string destination)
        {
            var route = await _googleMapsService.GetRouteAsync(origin, destination);
            return Ok(route);
        }
    }
}
