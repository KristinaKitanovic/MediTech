using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookListRazor.Model
{
    public class UserCart
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        public int UserId {  get; set; }
        [Required]
        public double OriginalPrice {  get; set; }
        [Required]
        public double FinalPrice {  get; set; }
        
    }
}
