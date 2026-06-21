using DomainLayer.DashboardModule;
using DomainLayer.Models.FeedbackModule;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.InfoBankModule;
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

        // NewInfoBanks:
        public DbSet<AttractionInfo> AttractionInfos { get; set; }
        public DbSet<AttractionRating> AttractionRatings { get; set; }
        public DbSet<HotelInfo> HotelInfos { get; set; }
        public DbSet<RestaurantInfo> RestaurantInfos { get; set; }
        public DbSet<FoodRecipe> FoodRecipes { get; set; }

        // Dashboard : 
        public DbSet<DashboardUser> DashboardUsers { get; set; }
        public DbSet<OwnerService> OwnerServices { get; set; }
        // Feedback :
        public DbSet<UserFeedback> UserFeedbacks { get; set; }

        #region Identity Module
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; } = default!;
        #endregion
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
                .HasOne(t => t.Attraction)
                .WithMany()
                .HasForeignKey(t => t.AttractionInfoId)
                .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<Plan>(entity =>
            {
                // الربط مع الـ Submission
                entity.HasOne(p => p.UserFormSubmission)
               .WithMany(s => s.Plans) // لازم الاسم ده يطابق اللي في كلاس UserFormSubmission
               .HasForeignKey(p => p.UserFormSubmissionId)
               .OnDelete(DeleteBehavior.Restrict);// حل مشكلة الـ Cascade اللي ظهرتلك

                // حل شامل لكل مشاكل الـ Decimal في جدول الـ Plan
                entity.Property(p => p.TotalPriceEGP).HasPrecision(18, 2);
                entity.Property(p => p.TotalEstimatedCost).HasPrecision(18, 2);
                entity.Property(p => p.TotalBudget).HasPrecision(18, 2);
            });

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
               .HasForeignKey(m => m.VoiceTranslationSessionId) 
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

            modelBuilder.Entity<PlanMeal>()
               .Property(p => p.Cost)
               .HasPrecision(10, 2);

            modelBuilder.Entity<Plan>()
                .Property(p => p.TotalBudget)
                .HasPrecision(18, 2);

            // مثال لجدول الـ AttractionInfo
            modelBuilder.Entity<AttractionInfo>(entity =>
            {
                entity.Property(e => e.ForeignerAdultPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.EgyptianAdultPrice).HasColumnType("decimal(18,2)");
                // كرر ده لكل حقول الـ decimal اللي ظهرت في الـ Log
            });

            // مثال لجدول الـ Plan (عشان الـ Budget)
            modelBuilder.Entity<Plan>()
                .Property(p => p.TotalBudget).HasColumnType("decimal(18,2)");
            #endregion

            #region 14- Dashboard Relationships
            modelBuilder.Entity<DashboardUser>()
                .HasMany(u => u.Services)
                .WithOne(s => s.Owner)
                .HasForeignKey(s => s.DashboardUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DashboardUser>()
                .HasIndex(u => u.Email)
                .IsUnique();
            #endregion

        }

    }
}
