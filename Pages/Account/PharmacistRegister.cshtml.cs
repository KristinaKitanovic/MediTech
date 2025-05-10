using BookListRazor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BookListRazor.Pages.Account
{
    public class PharmacistRegisterModel : PageModel
    {
        public readonly ApplicationDbContext db;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string UserPassword { get; set; }
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; } = string.Empty;
        public PharmacistRegisterModel(ApplicationDbContext db)
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
                var existingUsername = db.Users.FirstOrDefault(u => u.Username == Username);
                if (existingUsername != null)
                {
                    ErrorMessage = "Username already exists. Try another one.";
                    return Page();
                }

                var newUser = new User
                {
                    Username = Username,
                    Email = Email,
                    UserPassword = UserPassword,
                    Role = 'P'
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                HttpContext.Session.SetInt32("UserId", newUser.Id);
                HttpContext.Session.SetString("UserEmail", newUser.Email);
                return RedirectToPage("/Admin/PharmacistDashboard");

            }
            return Page();
        }
    }
}
