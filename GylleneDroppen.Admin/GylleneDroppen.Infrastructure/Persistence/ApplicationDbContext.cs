using GylleneDroppen.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public new DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Whisky> Whiskies { get; set; }
    public DbSet<TastingHistory> TastingHistories { get; set; }
    public DbSet<TastingEvent> TastingEvents { get; set; }
    public DbSet<TastingEventParticipant> TastingEventParticipants { get; set; }
    public DbSet<TastingEventWhisky> TastingEventWhiskies { get; set; }
    public DbSet<MembershipPeriod> MembershipPeriods { get; set; }
    public DbSet<UserMembership> UserMemberships { get; set; }
    public DbSet<UserTrialUsage> UserTrialUsages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure PostgreSQL to use UTC timestamps
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Whisky configuration
        builder.Entity<Whisky>(entity =>
        {
            entity.HasKey(w => w.Id);

            entity.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(w => w.Distillery)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(w => w.Abv)
                .HasPrecision(5, 2);

            entity.Property(w => w.Price)
                .HasPrecision(10, 2);

            entity.Property(w => w.Region)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Type)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Country)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(w => w.Color)
                .HasMaxLength(500);

            entity.Property(w => w.Nose)
                .HasMaxLength(1000);

            entity.Property(w => w.Palate)
                .HasMaxLength(1000);

            entity.Property(w => w.Finish)
                .HasMaxLength(1000);

            entity.Property(w => w.ImagePath)
                .HasMaxLength(500);

            entity.Property(w => w.CreatedByUserId)
                .IsRequired();

            // Relationships
            entity.HasOne(w => w.CreatedByUser)
                .WithMany()
                .HasForeignKey(w => w.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(w => w.UpdatedByUser)
                .WithMany()
                .HasForeignKey(w => w.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(w => new { w.Name, w.Distillery })
                .IsUnique()
                .HasDatabaseName("IX_Whisky_Name_Distillery");

            entity.HasIndex(w => w.Country)
                .HasDatabaseName("IX_Whisky_Country");

            entity.HasIndex(w => w.Region)
                .HasDatabaseName("IX_Whisky_Region");

            entity.HasIndex(w => w.Type)
                .HasDatabaseName("IX_Whisky_Type");

            entity.HasIndex(w => w.CreatedDate)
                .HasDatabaseName("IX_Whisky_CreatedDate");
        });

        // TastingHistory configuration
        builder.Entity<TastingHistory>(entity =>
        {
            entity.HasKey(th => th.Id);

            entity.Property(th => th.EventTitle)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(th => th.Notes)
                .HasMaxLength(1000);

            entity.Property(th => th.OrganizedByUserId)
                .IsRequired();

            // Relationships
            entity.HasOne(th => th.Whisky)
                .WithMany(w => w.TastingHistories)
                .HasForeignKey(th => th.WhiskyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(th => th.OrganizedByUser)
                .WithMany()
                .HasForeignKey(th => th.OrganizedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(th => th.WhiskyId)
                .HasDatabaseName("IX_TastingHistory_WhiskyId");

            entity.HasIndex(th => th.TastingDate)
                .HasDatabaseName("IX_TastingHistory_TastingDate");

            entity.HasIndex(th => th.OrganizedByUserId)
                .HasDatabaseName("IX_TastingHistory_OrganizedByUserId");
        });

        // TastingEvent configuration
        builder.Entity<TastingEvent>(entity =>
        {
            entity.HasKey(te => te.Id);

            entity.Property(te => te.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(te => te.Description)
                .HasMaxLength(1000);

            entity.Property(te => te.Location)
                .HasMaxLength(200);

            entity.Property(te => te.OrganizedByUserId)
                .IsRequired();

            // Relationships
            entity.HasOne(te => te.OrganizedByUser)
                .WithMany()
                .HasForeignKey(te => te.OrganizedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(te => te.EventDate)
                .HasDatabaseName("IX_TastingEvent_EventDate");

            entity.HasIndex(te => te.OrganizedByUserId)
                .HasDatabaseName("IX_TastingEvent_OrganizedByUserId");

            entity.HasIndex(te => te.IsPublic)
                .HasDatabaseName("IX_TastingEvent_IsPublic");
        });

        // TastingEventParticipant configuration
        builder.Entity<TastingEventParticipant>(entity =>
        {
            entity.HasKey(tep => tep.Id);

            entity.Property(tep => tep.TastingEventId)
                .IsRequired();

            entity.Property(tep => tep.UserId)
                .IsRequired();

            entity.Property(tep => tep.Notes)
                .HasMaxLength(1000);

            // Relationships
            entity.HasOne(tep => tep.TastingEvent)
                .WithMany(te => te.Participants)
                .HasForeignKey(tep => tep.TastingEventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tep => tep.User)
                .WithMany()
                .HasForeignKey(tep => tep.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint - user can only be registered once per event
            entity.HasIndex(tep => new { tep.TastingEventId, tep.UserId })
                .IsUnique()
                .HasDatabaseName("IX_TastingEventParticipant_TastingEventId_UserId");

            // Indexes
            entity.HasIndex(tep => tep.TastingEventId)
                .HasDatabaseName("IX_TastingEventParticipant_TastingEventId");

            entity.HasIndex(tep => tep.UserId)
                .HasDatabaseName("IX_TastingEventParticipant_UserId");
        });

        // TastingEventWhisky configuration
        builder.Entity<TastingEventWhisky>(entity =>
        {
            entity.HasKey(tew => tew.Id);

            entity.Property(tew => tew.TastingEventId)
                .IsRequired();

            entity.Property(tew => tew.WhiskyId)
                .IsRequired();

            entity.Property(tew => tew.AddedByUserId)
                .IsRequired();

            entity.Property(tew => tew.Notes)
                .HasMaxLength(1000);

            // Relationships
            entity.HasOne(tew => tew.TastingEvent)
                .WithMany(te => te.TastingEventWhiskies)
                .HasForeignKey(tew => tew.TastingEventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tew => tew.Whisky)
                .WithMany()
                .HasForeignKey(tew => tew.WhiskyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(tew => tew.AddedByUser)
                .WithMany()
                .HasForeignKey(tew => tew.AddedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint - whisky can only be added once per event
            entity.HasIndex(tew => new { tew.TastingEventId, tew.WhiskyId })
                .IsUnique()
                .HasDatabaseName("IX_TastingEventWhisky_TastingEventId_WhiskyId");

            // Indexes
            entity.HasIndex(tew => tew.TastingEventId)
                .HasDatabaseName("IX_TastingEventWhisky_TastingEventId");

            entity.HasIndex(tew => tew.WhiskyId)
                .HasDatabaseName("IX_TastingEventWhisky_WhiskyId");

            entity.HasIndex(tew => tew.Order)
                .HasDatabaseName("IX_TastingEventWhisky_Order");
        });

        // MembershipPeriod configuration
        builder.Entity<MembershipPeriod>(entity =>
        {
            entity.HasKey(mp => mp.Id);

            entity.Property(mp => mp.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(mp => mp.DurationInMonths)
                .IsRequired();

            entity.Property(mp => mp.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(mp => mp.CreatedByUserId)
                .IsRequired();

            // Relationships
            entity.HasOne(mp => mp.CreatedByUser)
                .WithMany()
                .HasForeignKey(mp => mp.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(mp => mp.UpdatedByUser)
                .WithMany()
                .HasForeignKey(mp => mp.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(mp => mp.IsActive)
                .HasDatabaseName("IX_MembershipPeriod_IsActive");

            entity.HasIndex(mp => mp.DurationInMonths)
                .HasDatabaseName("IX_MembershipPeriod_DurationInMonths");
        });

        // UserMembership configuration
        builder.Entity<UserMembership>(entity =>
        {
            entity.HasKey(um => um.Id);

            entity.Property(um => um.UserId)
                .IsRequired();

            entity.Property(um => um.MembershipPeriodId)
                .IsRequired();

            entity.Property(um => um.AmountPaid)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(um => um.Notes)
                .HasMaxLength(500);

            entity.Property(um => um.CreatedByUserId)
                .IsRequired();

            // Relationships
            entity.HasOne(um => um.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(um => um.MembershipPeriod)
                .WithMany(mp => mp.UserMemberships)
                .HasForeignKey(um => um.MembershipPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(um => um.CreatedByUser)
                .WithMany()
                .HasForeignKey(um => um.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(um => um.UserId)
                .HasDatabaseName("IX_UserMembership_UserId");

            entity.HasIndex(um => um.StartDate)
                .HasDatabaseName("IX_UserMembership_StartDate");

            entity.HasIndex(um => um.EndDate)
                .HasDatabaseName("IX_UserMembership_EndDate");

            entity.HasIndex(um => um.IsActive)
                .HasDatabaseName("IX_UserMembership_IsActive");

            entity.HasIndex(um => new { um.UserId, um.EndDate })
                .HasDatabaseName("IX_UserMembership_UserId_EndDate");
        });

        // UserTrialUsage configuration
        builder.Entity<UserTrialUsage>(entity =>
        {
            entity.HasKey(utu => utu.Id);

            entity.Property(utu => utu.UserId)
                .IsRequired();

            entity.Property(utu => utu.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(utu => utu.Notes)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(utu => utu.User)
                .WithOne(u => u.TrialUsage)
                .HasForeignKey<UserTrialUsage>(utu => utu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(utu => utu.TrialUsedForEvent)
                .WithMany()
                .HasForeignKey(utu => utu.TrialUsedForEventId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            entity.HasIndex(utu => utu.UserId)
                .IsUnique()
                .HasDatabaseName("IX_UserTrialUsage_UserId");

            entity.HasIndex(utu => utu.Email)
                .HasDatabaseName("IX_UserTrialUsage_Email");

            entity.HasIndex(utu => utu.HasUsedTrial)
                .HasDatabaseName("IX_UserTrialUsage_HasUsedTrial");
        });
    }
}