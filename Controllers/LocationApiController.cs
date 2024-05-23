using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadrifoglioAPI.Data;
using QuadrifoglioAPI.DTOs;
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
        private readonly ApplicationDbContext _context;

        public LocationApiController(GoogleMapsService googleMapsService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _googleMapsService = googleMapsService;
            _userManager = userManager;
            _context = context;
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
        public async Task<IActionResult> AddNewAddress([FromBody] AddressDTO addressDto)
        {
            var userId = addressDto.UserId;
            if (userId == null)
            {
                return Unauthorized();
            }

            var newAddress = new Address
            {
                Street = addressDto.Street,
                PostalCode = addressDto.PostalCode,
                City = addressDto.City,
                UserId = userId
            };

            _context.Add(newAddress);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
