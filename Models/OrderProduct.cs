using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuadrifoglioAPI.Models
{
    public class OrderProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderProductId { get; set; }

        public int Quantity { get; set; }

        public int FkOrderId { get; set; }

        [ForeignKey("Product")]
        public int FkProductId { get; set; }
        public Product Product { get; set; }
        public IEnumerable<Ingredient>? Ingredients { get; set; }
    }
}
