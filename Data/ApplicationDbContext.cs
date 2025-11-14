using Microsoft.EntityFrameworkCore;
using PersonalDetailsAPI.Models.Entities;

namespace PersonalDetailsAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PersonalDetail> PersonalDetails { get; set; }
    public DbSet<WifeDetail> WifeDetails { get; set; }
    public DbSet<ChildDetail> ChildDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure PersonalDetail
        modelBuilder.Entity<PersonalDetail>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);

            // MANDATORY FIELDS
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
            // ResidentialStatus is enum, already required by default

            // OPTIONAL FIELDS
            entity.Property(e => e.AlternateNumber).HasMaxLength(10);
            entity.Property(e => e.AadharNumber).HasMaxLength(12);
            entity.Property(e => e.EmailId).HasMaxLength(100);
            entity.Property(e => e.CasteGroup).HasMaxLength(50);
            entity.Property(e => e.Qualification).HasMaxLength(50);
            entity.Property(e => e.BloodGroup).HasMaxLength(10);
            entity.Property(e => e.OccupationDetail).HasMaxLength(200);
            entity.Property(e => e.FatherName).HasMaxLength(100);
            entity.Property(e => e.FatherOccupation).HasMaxLength(100);
            entity.Property(e => e.MotherName).HasMaxLength(100);
            entity.Property(e => e.MotherOccupation).HasMaxLength(100);
            entity.Property(e => e.Feedback).HasMaxLength(1000);

            entity.HasMany(e => e.WifeDetails)
                .WithOne(w => w.PersonalDetail)
                .HasForeignKey(w => w.PersonalDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.ChildDetails)
                .WithOne(c => c.PersonalDetail)
                .HasForeignKey(c => c.PersonalDetailId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure WifeDetail
        modelBuilder.Entity<WifeDetail>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Occupation).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Native).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Caste).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Qualification).IsRequired().HasMaxLength(50);
            entity.Property(e => e.BloodGroup).IsRequired().HasMaxLength(10);
        });

        // Configure ChildDetail
        modelBuilder.Entity<ChildDetail>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Qualification).IsRequired().HasMaxLength(50);
            entity.Property(e => e.BloodGroup).IsRequired().HasMaxLength(10);
        });
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedDate = DateTime.UtcNow;
            }
        }
    }
}
