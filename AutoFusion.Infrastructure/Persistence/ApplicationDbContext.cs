using AutoFusion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AutoFusion.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Dealership> Dealerships { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Sale> Sales { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Manufacturer>()
               .HasKey(m => m.ManufacturerId);

        builder.Entity<Manufacturer>()
               .Property(m => m.ManufacturerId)
               .ValueGeneratedOnAdd();

        builder.Entity<Manufacturer>()
               .HasIndex(m => m.Name)
               .IsUnique();

        builder.Entity<Vehicle>()
               .HasKey(v => v.VehicleId);

        builder.Entity<Vehicle>()
               .Property(v => v.VehicleId)
               .ValueGeneratedOnAdd();

        builder.Entity<Vehicle>()
               .HasIndex(v => v.Model);

        builder.Entity<Vehicle>()
               .HasOne(v => v.Manufacturer)
               .WithMany()
               .HasForeignKey(v => v.ManufacturerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Dealership>()
               .HasKey(d => d.DealershipId);

        builder.Entity<Dealership>()
               .Property(d => d.DealershipId)
               .ValueGeneratedOnAdd();

        builder.Entity<Dealership>()
               .HasIndex(d => d.Name)
               .IsUnique();

        builder.Entity<Customer>()
               .HasKey(c => c.CustomerId);

        builder.Entity<Customer>()
               .Property(c => c.CustomerId)
               .ValueGeneratedOnAdd();

        builder.Entity<Customer>()
               .Property(c => c.CPF)
               .HasColumnName("CPF")
               .HasMaxLength(11)
               .IsRequired();

        builder.Entity<Customer>()
               .HasIndex(c => c.CPF)
               .IsUnique();

        builder.Entity<Sale>()
               .HasKey(s => s.SaleId);

        builder.Entity<Sale>()
               .Property(s => s.SaleId)
               .ValueGeneratedOnAdd();

        builder.Entity<Sale>()
               .HasOne(s => s.Vehicle)
               .WithMany()
               .HasForeignKey(s => s.VehicleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Sale>()
               .HasOne(s => s.Dealership)
               .WithMany()
               .HasForeignKey(s => s.DealershipId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Sale>()
               .HasOne(s => s.Customer)
               .WithMany()
               .HasForeignKey(s => s.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
