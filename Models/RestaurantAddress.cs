using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuadrifoglioAPI.Models
{
    public class RestaurantAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Street can't be longer than 100 characters")]
        public string Street { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Postal code can't be longer than 10 characters")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "City can't be longer than 50 characters")]
        public string City { get; set; }
    }
}
