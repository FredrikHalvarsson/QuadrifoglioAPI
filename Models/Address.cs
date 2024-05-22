using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuadrifoglioAPI.Models
{
    public class Address
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

        [ForeignKey("User")]
        public string UserId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser? User { get; set; }
    }
}
