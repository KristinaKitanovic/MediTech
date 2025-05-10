using BookListRazor.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;


namespace BookListRazor.Pages.Shop
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext db;

        [BindProperty]
        public string DiscountCode { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string City { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public CartModel(ApplicationDbContext db)
        {
            this.db = db;
        }    
        public async Task OnGetAsync()
        {
            var user = HttpContext.Session.GetInt32("UserId");

            if (!user.HasValue)
            {
                return;
            }
            CartItems = await db.CartItems
                .Where(c => c.UserId == user.Value.ToString())
                .Include(c => c.Product)
                .ToListAsync();

            var usercart = await db.UserCarts.FirstOrDefaultAsync(uc => uc.UserId == user.Value);
            ViewData["Price"] = Math.Round(usercart.FinalPrice,2);
            if (usercart.OriginalPrice != usercart.FinalPrice)
            {
                ViewData["IsDiscounted"] = true;
            }
            else
            {
                ViewData["IsDiscounted"] = false;
            }
            foreach(var item in CartItems)
            {
                if(item.Product.Quantity == 0)
                {
                    item.IsOutOfStock = true;
                }
                else if(item.Product.Quantity < item.Quantity)
                {
                    item.IsAvailable = false;
                }
                else
                {
                    item.IsAvailable = true;
                }
            }
        }

        public async Task<IActionResult> OnPostCalculateDiscount()
        {
            var user = HttpContext.Session.GetInt32("UserId");

            if (!user.HasValue || string.IsNullOrEmpty(DiscountCode))
            {
                return RedirectToPage();
            }

            
            var discount = await db.DiscountCodes.FirstOrDefaultAsync(c => c.Code == DiscountCode);
            if (discount == null)
            {
                return RedirectToPage();
            }

            var userCart = await db.UserCarts.FirstOrDefaultAsync(uc => uc.UserId == user.Value);
            userCart.FinalPrice = Math.Round(userCart.OriginalPrice * (100 - discount.Discount) / 100,2);

            await db.SaveChangesAsync();
            ViewData["Price"] = userCart.FinalPrice;

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveFromCart(int productId)
        {
            var user = HttpContext.Session.GetInt32("UserId");
            var cartItem = await db.CartItems
             .Include(c => c.Product) 
             .FirstOrDefaultAsync(c => c.UserId == user.Value.ToString() && c.ProductId == productId);


            db.CartItems.Remove(cartItem);
            
            var userCart = await db.UserCarts.FirstOrDefaultAsync(uc => uc.UserId == user.Value);
            userCart.OriginalPrice -= cartItem.Quantity * cartItem.Product.Price;
            userCart.FinalPrice = userCart.OriginalPrice;

            await db.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostOrderAsync()
        {
            var user = HttpContext.Session.GetInt32("UserId");

            if (!user.HasValue)
            {
                return Page(); 
            }

            
            CartItems = await db.CartItems
                .Where(c => c.UserId == user.Value.ToString())
                .Include(c => c.Product)
                .ToListAsync();

            foreach(var item in CartItems)
            {
                if(item.Product.Quantity < item.Quantity)
                {
                    return RedirectToPage();
                }
                var product = await db.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                product.Quantity -= item.Quantity;
            }
            
            var emailService = new EmailService();
            string userEmail = HttpContext.Session.GetString("UserEmail");
            string subject = "Thank You for Your Order";
            string body = "Thank you for shopping with MediTech. Your order has been received" +
                " and will be processed soon! \n Kind Regards, MediTech Pharmacy";
            await emailService.SendEmailAsync(userEmail, subject, body);

            string companyEmail = "yourmeditechcompany@gmail.com";
            subject = "New Order";
            body = "New order has arrived! " + "\n\n";
            body += "Customer Email: " + HttpContext.Session.GetString("UserEmail") + "\n";
            body += $"Customer Address: {Address}, {City} \n";
            body += $"Customer Phone Number: {PhoneNumber}\n\n";
            body += "Products ordered: \n\n";

            foreach (var item in CartItems)
            {
                body += $"{item.Product.Label} - {item.Quantity} pcs\n";
            }

            var usercart = await db.UserCarts.FirstOrDefaultAsync(uc => uc.UserId == user.Value);
            body += $"\nTotal Price: {usercart.FinalPrice}$";
            await emailService.SendEmailAsync(companyEmail, subject, body);
            db.CartItems.RemoveRange(CartItems);
            usercart.OriginalPrice = 0;
            usercart.FinalPrice = 0;
            await db.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
