using Microsoft.EntityFrameworkCore;
using SOA.features.auth.models;
using SOA.features.image.models;
using SOA.features.location.models;
using SOA.features.properties.models;
using SOA.features.property.models;
using SOA.features.services.models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // === AUTH ===
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;

    // === LOCATION ===
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Province> Provinces { get; set; } = null!;
    public DbSet<District> Districts { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;

    // === PROPERTIES ===
    public DbSet<Property> Properties { get; set; } = null!;
    public DbSet<ResidentialProperty> ResidentialProperties { get; set; } = null!;
    public DbSet<CommercialProperty> CommercialProperties { get; set; } = null!;

    // === SERVICES ===
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<ServiceToProperty> ServicesToProperties { get; set; } = null!;

    // === IMAGES ===
    public DbSet<Image> Images { get; set; } = null!;

    // === TEST ===
    public DbSet<TestEntity> TestEntities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============================
        // LOCATION RELATIONSHIPS
        // ============================

        modelBuilder.Entity<Province>()
            .HasOne(p => p.Department)
            .WithMany(d => d.Provinces)
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<District>()
            .HasOne(d => d.Province)
            .WithMany(p => p.Districts)
            .HasForeignKey(d => d.ProvinceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<District>()
            .HasOne(d => d.Department)
            .WithMany()
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================
        // PROPERTY RELATIONSHIPS
        // ============================

        // Property -> Location (N:1)
        modelBuilder.Entity<Property>()
            .HasOne(p => p.Location)
            .WithMany()
            .HasForeignKey(p => p.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Property -> ResidentialProperty (1:1 Shared Key)
        modelBuilder.Entity<Property>()
            .HasOne(p => p.ResidentialProperty)
            .WithOne(r => r.Property)
            .HasForeignKey<ResidentialProperty>(r => r.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Property -> CommercialProperty (1:1 Shared Key)
        modelBuilder.Entity<Property>()
            .HasOne(p => p.CommercialProperty)
            .WithOne(c => c.Property)
            .HasForeignKey<CommercialProperty>(c => c.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Property -> Images (1:N)
        modelBuilder.Entity<Property>()
            .HasMany(p => p.Images)
            .WithOne(i => i.Property)
            .HasForeignKey(i => i.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Property -> ServicesToProperties (1:N)
        modelBuilder.Entity<Property>()
            .HasMany(p => p.ServicesToProperties)
            .WithOne(sp => sp.Property)
            .HasForeignKey(sp => sp.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        // ============================
        // SERVICE RELATIONSHIPS
        // ============================

        modelBuilder.Entity<ServiceToProperty>()
            .HasKey(sp => new { sp.PropertyId, sp.ServiceId });

        modelBuilder.Entity<ServiceToProperty>()
            .HasOne(sp => sp.Property)
            .WithMany(p => p.ServicesToProperties)
            .HasForeignKey(sp => sp.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceToProperty>()
            .HasOne(sp => sp.Service)
            .WithMany(s => s.ServiceToProperties)
            .HasForeignKey(sp => sp.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================
        // IMAGE RELATIONSHIPS
        // ============================

        modelBuilder.Entity<Image>()
            .HasOne(i => i.Property)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
