using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuadrifoglioAPI.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Restaurant name can't be longer than 100 characters")]
        public string Name { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        [JsonIgnore]
        public RestaurantAddress? Address { get; set; }
    }
}
