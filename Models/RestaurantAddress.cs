using System.ComponentModel.DataAnnotations;

namespace QuadrifoglioAPI.Models
{
    public class RestaurantAddress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Street can't be longer than 100 characters")]
        public string Street { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "City can't be longer than 50 characters")]
        public string City { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "State can't be longer than 50 characters")]
        public string State { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Postal code can't be longer than 10 characters")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Country can't be longer than 50 characters")]
        public string Country { get; set; }
    }
}
