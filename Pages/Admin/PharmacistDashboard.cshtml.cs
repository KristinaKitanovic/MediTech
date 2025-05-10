using BookListRazor.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookListRazor.Pages.Admin
{
    public class PharmacistDashboardModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public class UserWithMessages
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public int UnreadCount { get; set; }

            public DateTime LastMessageDate { get; set; }
        }

        public IList<UserWithMessages> UsersWithMessages { get; set; } = new List<UserWithMessages>();
        public IEnumerable<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

        public IList<Products> Products { get; set; }

        [BindProperty]
        public int IdInput { get; set; }
        [BindProperty]
        public string Label { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string Type { get; set; }
        [BindProperty]
        public double Price { get; set; }
        [BindProperty]
        public DateTime ExpirationDate { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
        [BindProperty]
        public string MessageInput { get; set; }
        [BindProperty]
        public int SelectedUserId { get; set; }
        [BindProperty]
        public string GeneratedCode { get; set; }

        [BindProperty]
        public int DiscountPercent { get; set; }

        public string ImagePath { get; set; }
        public bool ShowChat { get; set; }
        public string errorMessage { get; set; } = string.Empty;
        public PharmacistDashboardModel(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task OnGetAsync()
        {
            Products = await db.Products.ToListAsync();
            if (Products == null || !Products.Any())
            {
                Products = new List<Products>();
            }
            var chatMessages = await db.ChatMessages
             .Include(m => m.User)
             .ToListAsync(); 

            UsersWithMessages = chatMessages
                .GroupBy(m => m.CustomerId)
                .Select(g => new UserWithMessages
                {
                    UserId = g.Key,
                    Username = g.FirstOrDefault().User.Username,
                    UnreadCount = g.Count(m => m.CustomerId == m.UserId && !m.IsRead),
                    LastMessageDate = g.Max(m => m.Time)  
                })
                .OrderByDescending(u => u.LastMessageDate) 
                .ToList();
        }
       
        public async Task<IActionResult> OnPostSaveCodeAsync()
        {
            var newDiscount = new DiscountCodes
            {
                Code = GeneratedCode,
                Discount = DiscountPercent
            };
            await db.DiscountCodes.AddAsync(newDiscount);
            await db.SaveChangesAsync();
            return RedirectToPage();
        }
        public async Task<IActionResult> OnGetLoadUserMessages(int id)
        {
            ShowChat = true;
            SelectedUserId = id;
            ChatMessages = await db.ChatMessages
                .Where(c => c.CustomerId == id)
                .Include(c => c.User)
                .OrderBy(c => c.Time)
                .ToListAsync();
            var unreadMessages = await db.ChatMessages
                .Where(c => c.CustomerId == id && c.UserId == c.CustomerId && !c.IsRead)
                .ToListAsync();
            foreach(var message in unreadMessages)
            {
                message.IsRead = true;
            }
            await db.SaveChangesAsync();
            await OnGetAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostSendMessage()
        {
            var user = HttpContext.Session.GetInt32("UserId");
            if (!user.HasValue)
            {
                return RedirectToPage();
            }
            var newMessage = new ChatMessage
            {
                UserId = user.Value,
                MessageText = MessageInput,
                CustomerId = SelectedUserId,
                Time = DateTime.Now,
                IsRead = false
            };
            ShowChat = true;
            db.ChatMessages.Add(newMessage);
            await db.SaveChangesAsync();
            Products = await db.Products.ToListAsync();
            await OnGetLoadUserMessages(SelectedUserId);
            return Page();
        }
        public IActionResult OnGetCloseChat()
        {
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (Image != null && Image.Length > 0)
            {
                
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products/", Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    
                    await Image.CopyToAsync(stream);
                }
                ImagePath = "/images/products/" + Image.FileName;
            }
            if (ModelState.IsValid)
            {
               
                var action = Request.Form["Action"];  
                if (action == "add")
                {
                    var newProduct = new Products
                    {
                        Label = Label,
                        Price = Price,
                        Description = Description,
                        Type = Type,
                        Image = ImagePath,
                        ExpirationDate = ExpirationDate,
                        Quantity = Quantity
                    };

                    db.Products.Add(newProduct);
                    await db.SaveChangesAsync();
                    return RedirectToPage();
                }
                else if (action == "update")
                {

                    var product = await db.Products.FirstOrDefaultAsync(p => p.Id == IdInput);
                    if (product != null)
                    {
                        product.Label = Label;
                        product.Price = Price;
                        product.Description = Description;
                        product.Type = Type;
                        product.ExpirationDate = ExpirationDate;
                        product.Quantity = Quantity;

                        
                        if (Image != null && Image.Length > 0)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products/", Image.FileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await Image.CopyToAsync(stream);
                            }

                            product.Image = "/images/products/" + Image.FileName;
                        }

                        db.Products.Update(product);
                        await db.SaveChangesAsync();
                    }
                    return RedirectToPage();
                }
                else if (action == "delete")
                {
                   
                    var product = await db.Products.FirstOrDefaultAsync(p => p.Id == IdInput);
                    if (product != null)
                    {
                        db.Products.Remove(product);
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToPage();
            }

            return Page();
        }
    }
}
