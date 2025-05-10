using Microsoft.EntityFrameworkCore;
using BookListRazor.Model;

namespace BookListRazor.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Products> Products { get; set; }

        public DbSet<DiscountCodes> DiscountCodes { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<UserCart> UserCarts { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
