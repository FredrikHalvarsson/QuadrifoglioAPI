using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuadrifoglioAPI.Models
{
    public enum Option
    {
        Add,
        Remove,
        Replace
    }
    public class Ingredient
    {
        public int IngredientId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        public string IngredientName { get; set; }

        [Column(TypeName = "decimal(2.2)")]
        public decimal Price { get; set; }
        public Option IngredientOption { get; set; }
    }
}
