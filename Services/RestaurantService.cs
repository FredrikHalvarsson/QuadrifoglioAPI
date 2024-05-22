using Microsoft.EntityFrameworkCore;
using QuadrifoglioAPI.Data;
using QuadrifoglioAPI.Models;

namespace QuadrifoglioAPI.Services
{
    public class RestaurantService
    {
        private readonly ApplicationDbContext _context;

        public RestaurantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Restaurant> GetAllRestaurants()
        {
            return _context.Restaurants.Include(r => r.Address).ToList();
        }
    }
}
