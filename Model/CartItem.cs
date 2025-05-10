using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookListRazor.Model
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ProductId { get; set; } 

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public virtual Products Product { get; set; }
        [NotMapped]
        public bool IsAvailable { get; set; }
        [NotMapped]
        public bool IsOutOfStock { get; set; }
    }
}
