using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuadrifoglioAPI.Models
{
    public enum Status
    {
        Unsent,
        Pending,
        Preparing,
        Delivering,
        Delivered

    }
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public Status OrderStatus { get; set; } = 0;
        public DateTime OrderTime { get; set; } = DateTime.Now;
        public DateTime EstimatedDelivery => OrderTime.AddMinutes(40);

        [ForeignKey("Customer")]
        public string FkCustomerId { get; set; }
        [JsonIgnore]
        public ApplicationUser? Customer { get; set; }


        [JsonIgnore]
        public List<OrderProduct>? OrderProducts { get; set; }
    }
}
