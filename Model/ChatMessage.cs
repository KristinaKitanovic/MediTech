using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace BookListRazor.Model
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId {  get; set; }

        [Required]
        public string MessageText {  get; set; }
        [Required]
        public int CustomerId {  get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public bool IsRead { get; set; }
        public User User { get; set; }
        
    }
}
