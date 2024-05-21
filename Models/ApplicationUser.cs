using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuadrifoglioAPI.Models
{
    public class ApplicationUser :IdentityUser
    {

        [StringLength(50, ErrorMessage ="Name can't be longer than 50 characters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LastName { get; set; }

        [StringLength(50, ErrorMessage = "Address can't be longer than 50 characters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Address { get; set; }

        //public double? Latitude { get; set; }
        //public double? Longitude { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<Order>? Orders { get; set; }
    }
}
