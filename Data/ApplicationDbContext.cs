using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuadrifoglioAPI.Models;

namespace QuadrifoglioAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //public DbSet<RestaurantAddress> RestaurantAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // Seed data
            SeedRestaurant(builder);
        }

        private void SeedRestaurant(ModelBuilder builder)
        {
            //Hudik
            var restaurantAddress = new RestaurantAddress
            {
                Id = 1,
                Street = "Kungsgatan 25",
                PostalCode = "824 43",
                City = "Hudiksvall"
            };

            var restaurant = new Restaurant
            {
                Id = 1,
                Name = "IlQuadrifoglio - Hudiksvall",
                AddressId = 1 // Foreign key for address
            };

            builder.Entity<RestaurantAddress>().HasData(restaurantAddress);
            builder.Entity<Restaurant>().HasData(restaurant);

            //Sundsvall
            var restaurantAddress2 = new RestaurantAddress
            {
                Id = 2,
                Street = "Storgatan 6",
                PostalCode = "852 30",
                City = "Sundsvall"
            };

            var restaurant2 = new Restaurant
            {
                Id = 2,
                Name = "IlQuadrifoglio - Sundsvall",
                AddressId = 2 // Foreign key for address
            };

            builder.Entity<RestaurantAddress>().HasData(restaurantAddress2);
            builder.Entity<Restaurant>().HasData(restaurant2);

            //Övik
            var restaurantAddress3 = new RestaurantAddress
            {
                Id = 3,
                Street = "Köpmangatan 3A",
                PostalCode = "891 63",
                City = "Örnsköldsvik"
            };

            var restaurant3 = new Restaurant
            {
                Id = 3,
                Name = "IlQuadrifoglio - Örnsköldsvik",
                AddressId = 3 // Foreign key for address
            };

            builder.Entity<RestaurantAddress>().HasData(restaurantAddress3);
            builder.Entity<Restaurant>().HasData(restaurant3);
        }

        //For more detailed error messages
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .LogTo(Console.WriteLine)
                .EnableDetailedErrors();
    }
}
