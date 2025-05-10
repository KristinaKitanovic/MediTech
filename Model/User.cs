using System.ComponentModel.DataAnnotations;

namespace BookListRazor.Model
{
    public class User
    {
        [Key] public int Id { get; set; }
        [Required] public string Username {get; set; }
        [Required] public string Email {get; set; }
        [Required] public string UserPassword {get; set; }
        public char Role {  get; set; }
    }
}
