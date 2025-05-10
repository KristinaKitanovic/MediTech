using BookListRazor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BookListRazor.Pages.Account
{
    public class PharmacistLoginModel : PageModel
    {
        public readonly ApplicationDbContext db;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string UserPassword { get; set; }
        [BindProperty]
        public string Email { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
        public PharmacistLoginModel(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                
                var user = db.Users.FirstOrDefault(u => u.Username == Username);
                if (user != null && user.Role == 'P' && user.UserPassword == UserPassword && user.Email == Email)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    return RedirectToPage("/Admin/PharmacistDashboard");
                }
                else
                {
                    ErrorMessage = "This Account does not exist.";
                    return Page();

                }
            }
            return Page();
        }
    }
}
