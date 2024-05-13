using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuadrifoglioAPI.Models
{
    public enum Status
    {
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
        public ApplicationUser? Customer { get; set; }


        public IEnumerable<OrderProduct> OrderProducts { get; set; }
    }
}
