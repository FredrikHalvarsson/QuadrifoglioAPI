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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // Seed data
            SeedRestaurant(builder);
            //SeedUser(builder);
        }

        private void SeedRestaurant(ModelBuilder builder)
        {
            var restaurantAddress = new RestaurantAddress
            {
                Id = 1,
                Street = "Kungsgatan 25",
                City = "Hudiksvall",
                State = "Gävleborg",
                PostalCode = "824 43",
                Country = "Sweden"
            };

            var restaurant = new Restaurant
            {
                Id = 1,
                Name = "IlQuadrifoglio - Hudiksvall",
                AddressId = 1 // Foreign key for address
            };

            builder.Entity<RestaurantAddress>().HasData(restaurantAddress);
            builder.Entity<Restaurant>().HasData(restaurant);
        }

        private void SeedUser(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "fredrik@user.com",
                NormalizedUserName = "FREDRIK@USER.COM",
                Email = "fredrik@user.com",
                NormalizedEmail = "FREDRIK@USER.COM",
                EmailConfirmed = true,
                FirstName = "Fredrik",
                LastName = "User",
                PasswordHash = hasher.HashPassword(null, "User@123")
            };

            var userAddress = new Address
            {
                Id = 2,
                Street = "Hövdingegatan 13A",
                City = "Hudiksvall",
                State = "Gävleborg",
                PostalCode = "824 43",
                Country = "Sweden",
                UserId = user.Id,
            };

            builder.Entity<ApplicationUser>().HasData(user);
            builder.Entity<Address>().HasData(userAddress);
        }

        //For more detailed error messages
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .LogTo(Console.WriteLine)
                .EnableDetailedErrors();
    }
}
