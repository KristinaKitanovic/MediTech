using BookListRazor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace BookListRazor.Pages.Shop
{
    public class FrontendModel : PageModel
    {
        private readonly ApplicationDbContext db;
        public IEnumerable<Products> Products { get; set; } = new List<Products>();
        public IEnumerable<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

        [BindProperty]
        public int ProductId { get; set; }

        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public string MessageInput { get; set; }
        public string ProductName { get; set; } = String.Empty;
        public bool ShowPopup { get; set; } = false;
        public bool ShowChat { get; set; } = false;
        public string errorMessage { get; set; } = String.Empty;
        public FrontendModel(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task OnGet()
        {
            Products = await db.Products.ToListAsync();
            var user = HttpContext.Session.GetInt32("UserId");
            ChatMessages = await db.ChatMessages
                .Where(c => c.CustomerId == user.Value)
                .Include(c => c.User)
                .OrderBy(c => c.Time)
                .ToListAsync();
        }
        public IActionResult OnPostCancel()
        {
            ShowPopup = false;
            return Page();
        }
        public IActionResult OnPostCloseChat()
        {
            ShowChat = false;
            return Page();
        }
        public async Task<IActionResult> OnPostSendMessage()
        {
            var user = HttpContext.Session.GetInt32("UserId");
            if(!user.HasValue)
            {
                return RedirectToPage();
            }
            var newMessage = new ChatMessage
            {
                UserId = user.Value,
                MessageText = MessageInput,
                CustomerId = user.Value,
                Time = DateTime.Now,
                IsRead = false
            };
            ShowChat = true;
            db.ChatMessages.Add(newMessage);
            await db.SaveChangesAsync();
            Products = await db.Products.ToListAsync();
            ChatMessages = await db.ChatMessages
                .Where(c => c.CustomerId == user.Value)
                .Include(c => c.User)
                .OrderBy(c => c.Time)
                .ToListAsync();
            return Page();

        }
       public async Task<IActionResult> OnPost()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Page();
            }
            var existing = await db.CartItems.FirstOrDefaultAsync(c => c.UserId == userId.ToString() && c.ProductId == ProductId);
            
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == ProductId);

            var availableAmount = product.Quantity;

            if(existing == null)
            {
                if(Quantity > availableAmount)
                {
                    errorMessage = "The requested quantity is currently not available in stock.";
                    ShowPopup = true;
                    ProductName = product.Label;
                    Products = await db.Products.ToListAsync();
                    return Page();
                }
                var cartitem = new CartItem
                {
                    UserId = userId.Value.ToString(),
                    ProductId = ProductId,
                    Quantity = Quantity
                };
                db.CartItems.Add(cartitem);
            }
            else
            {
                if(existing.Quantity + Quantity > availableAmount)
                {
                    errorMessage = "The requested quantity is currently not available in stock.";
                    ShowPopup = true;
                    ProductName = product.Label;
                    Products = await db.Products.ToListAsync();
                    return Page();
                }
                existing.Quantity += Quantity;  
            }

            var userCart = await db.UserCarts.FirstOrDefaultAsync(uc => uc.UserId == userId.Value);

            userCart.OriginalPrice += product.Price * Quantity;
            userCart.FinalPrice = userCart.OriginalPrice;
            await db.SaveChangesAsync();
            
            return RedirectToPage() ;
        }
    }
}
