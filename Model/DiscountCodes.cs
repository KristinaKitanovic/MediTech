using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookListRazor.Model
{
    public class DiscountCodes
    {
        [Key]
        public string Code { get; set; }
        [Required]
        public float Discount { get; set; }
    }
}
