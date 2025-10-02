using Microsoft.EntityFrameworkCore;
using DoAn_WebAPI.Models;
using DoAn_WebAPI.Models.DTOs;
namespace DoAn_WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboDetail> ComboDetails { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageImage> MessageImages { get; set; }
        // Các kết quả của stored procedure
        public DbSet<RevenueTodayDTO> RevenueTodayResults { get; set; }
        public DbSet<RevenueWeekDTO> RevenueWeekResults { get; set; }
        public DbSet<OrderCountDTO> OrderCountResults { get; set; }
        public DbSet<BestSellingItemDTO> BestSellingItemResults { get; set; }
        public DbSet<TopSellingItemMonthlyDTO> TopSellingItemMonthlyResults { get; set; }
        
        public DbSet<MenuItemRatingDTO> MenuItemRatingDTO { get; set; }
        public DbSet<RestaurantRatingDTO> RestaurantRatingDTO { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItemRatingDTO>().HasNoKey().ToView(null);
            modelBuilder.Entity<RestaurantRatingDTO>().HasNoKey().ToView(null);
            modelBuilder.Entity<RevenueTodayDTO>().HasNoKey();
            modelBuilder.Entity<RevenueWeekDTO>().HasNoKey();
            modelBuilder.Entity<OrderCountDTO>().HasNoKey();
            modelBuilder.Entity<BestSellingItemDTO>().HasNoKey();
            modelBuilder.Entity<TopSellingItemMonthlyDTO>().HasNoKey();

            modelBuilder.Entity<User>()
            .HasOne(u => u.Restaurant)
            .WithOne(r => r.User)
            .HasForeignKey<Restaurant>(r => r.UserID)
            .IsRequired();
            // Conversation
            modelBuilder.Entity<Conversation>()
                .HasKey(c => c.ConversationId);

            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message
            modelBuilder.Entity<Message>()
                .HasKey(m => m.MessageId);

            modelBuilder.Entity<Message>()
                .Property(m => m.Content)
                .HasMaxLength(2000);

            modelBuilder.Entity<Message>()
                .HasMany(m => m.Images)
                .WithOne(i => i.Message)
                .HasForeignKey(i => i.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            // MessageImage
            modelBuilder.Entity<MessageImage>()
                .HasKey(i => i.MessageImageId);

            modelBuilder.Entity<MessageImage>()
                .Property(i => i.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);
        }

    }
    
}

