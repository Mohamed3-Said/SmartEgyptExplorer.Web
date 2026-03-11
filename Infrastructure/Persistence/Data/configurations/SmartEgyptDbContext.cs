using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.Models.PlanModule;
using DomainLayer.Models.Models.Remaining_Modules;
using DomainLayer.Models.PlaceModule;
using DomainLayer.Models.PlanModule;
using DomainLayer.Models.PricingDetailsModule;
using DomainLayer.Models.Remaining_Modules;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data.configurations
{
    public class SmartEgyptDbContext(DbContextOptions<SmartEgyptDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        // Core
        public DbSet<Place> Places { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Attraction> Attractions { get; set; }

        // Pricing & Details
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomPrice> RoomPrices { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItemPrice> MenuItemPrices { get; set; }
        public DbSet<AttractionTicket> AttractionTickets { get; set; }
        public DbSet<TransportationOption> TransportationOptions { get; set; }

        // Plans
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanDay> PlanDays { get; set; }
        public DbSet<PlanActivity> PlanActivities { get; set; }

        // User Actions
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        // Info Bank
        public DbSet<InfoCategory> InfoCategories { get; set; }
        public DbSet<InfoItem> InfoItems { get; set; }

        // Form & AI
        public DbSet<UserFormSubmission> UserFormSubmissions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<AIResult> AIResults { get; set; }
        public DbSet<VoiceTranslationSession> VoiceTranslationSessions { get; set; }
        public DbSet<VoiceTranslationMessage> VoiceTranslationMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().ToTable("Users");
            #region 1- Place ↔ Hotel / Restaurant / Attraction (1–1)
            modelBuilder.Entity<Place>()
                 .HasOne(p => p.Hotel)
                 .WithOne(h => h.Place)
                 .HasForeignKey<Hotel>(h => h.PlaceId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Place>()
                .HasOne(p => p.Restaurant)
                .WithOne(r => r.Place)
                .HasForeignKey<Restaurant>(r => r.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Place>()
                .HasOne(p => p.Attraction)
                .WithOne(a => a.Place)
                .HasForeignKey<Attraction>(a => a.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);


            #endregion

            #region 2- Hotel → RoomType → RoomPrice
            modelBuilder.Entity<Hotel>()
                 .HasMany(h => h.RoomTypes)
                 .WithOne(rt => rt.Hotel)
                 .HasForeignKey(rt => rt.HotelId);

            modelBuilder.Entity<RoomType>()
                .HasMany(rt => rt.RoomPrices)
                .WithOne(rp => rp.RoomType)
                .HasForeignKey(rp => rp.RoomTypeId);

            #endregion

            #region 3- Restaurant → MenuItem → MenuItemPrice
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.MenuItems)
                .WithOne(mi => mi.Restaurant)
                .HasForeignKey(mi => mi.RestaurantId);

            modelBuilder.Entity<MenuItem>()
                .HasMany(mi => mi.MenuItemPrices)
                .WithOne(mp => mp.MenuItem)
                .HasForeignKey(mp => mp.MenuItemId);

            #endregion

            #region 4- Attraction → AttractionTicket

            modelBuilder.Entity<Attraction>()
                .HasMany(a => a.AttractionTickets)
                .WithOne(t => t.Attraction)
                .HasForeignKey(t => t.AttractionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttractionTicket>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            #endregion

            #region 5- Place → TransportationOption
            modelBuilder.Entity<Place>()
                .HasMany(p => p.TransportationOptions)
                .WithOne(t => t.Place)
                .HasForeignKey(t => t.PlaceId);

            #endregion

            #region 6- User → Review → Place
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Place)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region 7- User → Ticket → Place
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Place)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region 8- Plan → PlanDay → PlanActivity
            modelBuilder.Entity<Plan>()
                .HasOne(p => p.User)
                .WithMany(u => u.Plans)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Plan>()
                .HasMany(p => p.PlanDays)
                .WithOne(pd => pd.Plan)
                .HasForeignKey(pd => pd.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlanDay>()
                .HasMany(pd => pd.PlanActivities)
                .WithOne(pa => pa.PlanDay)
                .HasForeignKey(pa => pa.PlanDayId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlanActivity>()
                .HasOne(pa => pa.Place)
                .WithMany(p => p.PlanActivities)
                .HasForeignKey(pa => pa.PlaceId)
                .OnDelete(DeleteBehavior.SetNull);

            #endregion

            #region 9- InfoBank
            modelBuilder.Entity<InfoCategory>()
                .HasMany(c => c.InfoItems)
                .WithOne(i => i.Category)
                .HasForeignKey(i => i.CategoryId);
            #endregion

            #region 10- UserFormSubmission → UserAnswer → AIResult
            modelBuilder.Entity<UserFormSubmission>()
                .HasOne(s => s.User)
                .WithMany(u => u.FormSubmissions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade); // يفضل لو اليوزر اتمسح فورمه تتمسح

            modelBuilder.Entity<UserFormSubmission>()
                .HasMany(s => s.UserAnswers)
                .WithOne(a => a.Submission)
                .HasForeignKey(a => a.UserFormSubmissionId);

            modelBuilder.Entity<UserFormSubmission>()
                .HasMany(s => s.AIResults)
                .WithOne(r => r.Submission)
                .HasForeignKey(r => r.UserFormSubmissionId);

            #endregion

            #region 12- VoiceTranslationSession → VoiceTranslationMessage

            modelBuilder.Entity<VoiceTranslationMessage>()
                .HasKey(x => x.VoiceTranslationMessageId);

            modelBuilder.Entity<VoiceTranslationSession>()
               .HasMany(v => v.Messages)
               .WithOne(m => m.Session)
               .HasForeignKey(m => m.VoiceTranslationSessionId) // تعديل اسم الـ FK
               .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region // Explicit Primary Keys (Fix EF Core Detection Issues)
            // Explicit Primary Keys (Fix EF Core Detection Issues)

            modelBuilder.Entity<AttractionTicket>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<AIResult>()
                .HasKey(x => x.AIResultId);

            modelBuilder.Entity<UserAnswer>()
                .HasKey(x => x.UserAnswerId);

            modelBuilder.Entity<UserFormSubmission>()
                .HasKey(x => x.UserFormSubmissionId);

            modelBuilder.Entity<VoiceTranslationSession>()
                .HasKey(x => x.VoiceTranslationSessionId);


            modelBuilder.Entity<Ticket>()
                .HasKey(x => x.TicketId);

            modelBuilder.Entity<Review>()
                .HasKey(x => x.ReviewId);

            modelBuilder.Entity<PlanActivity>()
                .HasKey(x => x.PlanActivityId);

            #endregion

            #region 13- PlanActivity Time Precision & plan & budget cost precision
            modelBuilder.Entity<PlanActivity>()
                .Property(p => p.Cost)
                .HasPrecision(18, 2); 

            modelBuilder.Entity<PlanActivity>()
                .Property(p => p.TransportCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PlanBudgetItem>(entity =>
            {
                entity.Property(e => e.Spent).HasPrecision(18, 2);
                entity.Property(e => e.Limit).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Plan>(entity =>
            {
                entity.Property(e => e.TotalEstimatedCost).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Plan>()
                 .Property(p => p.TotalPriceEGP)
                 .HasPrecision(18, 2); 

            modelBuilder.Entity<Attraction>()
                .Property(a => a.Price)
                .HasPrecision(18, 2);
            #endregion


        }

    }
}
