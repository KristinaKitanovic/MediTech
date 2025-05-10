using System;
using System.ComponentModel.DataAnnotations;

namespace BookListRazor.Model
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Label { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
