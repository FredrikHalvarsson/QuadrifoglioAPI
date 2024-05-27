using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace QuadrifoglioAPI.Models
{
    public enum Type
    {
        Pizza,
        Sides,
        Beverage
    }
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        public string ProductName { get; set; }

        [StringLength(50, ErrorMessage = "Description can't be longer than 50 characters")]
        public string ProductDescription { get; set; }

        [Column(TypeName = "decimal(3.2)")]
        public decimal Price { get; set; }
        public Type ProductType { get; set; }
        public bool IsSpecial { get; set; }
        public decimal SalesTax { get; } = 0.12m;

        [JsonIgnore]
        public List<Ingredient>? Ingredients { get; set; }
    }
}
