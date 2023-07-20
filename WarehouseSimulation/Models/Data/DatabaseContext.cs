using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Windows.Input;

namespace WarehouseSimulation.Models.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<RackProduct> RacksProducts { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Dispatch> Dispatches { get; set; }
        public DbSet<DispatchProduct> DispatchesProducts { get; set; }
        public DbSet<Delivery> DeliveriesWindow { get; set; }
        public DbSet<DeliveryProduct> DeliveriesProducts { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DateOnlyNullableConverter dateOnlyNullableConverter = new DateOnlyNullableConverter();

            modelBuilder
                .Entity<Delivery>()
                .Property(d => d.ApprovalDate)
                .HasConversion(dateOnlyNullableConverter);
            modelBuilder
                .Entity<Delivery>()
                .Property(d => d.CreationDate)
                .HasConversion(dateOnlyNullableConverter);


            modelBuilder
                .Entity<Dispatch>()
                .Property(d => d.ApprovalDate)
                .HasConversion(dateOnlyNullableConverter);
            modelBuilder
                .Entity<Dispatch>()
                .Property(d => d.CreationDate)
                .HasConversion(dateOnlyNullableConverter);

            modelBuilder
                .Entity<RackProduct>()
                .HasOne(rp => rp.Product)
                .WithMany(p => p.RackProducts)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder
                .Entity<RackProduct>()
                .HasOne(rp => rp.Rack)
                .WithMany(r => r.RackProducts)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<DeliveryProduct>()
                .HasOne(rp => rp.Product)
                .WithMany(p => p.DeliveryProducts)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder
                .Entity<DeliveryProduct>()
                .HasOne(rp => rp.Delivery)
                .WithMany(d => d.DeliveryProducts)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .Entity<DispatchProduct>()
                .HasOne(rp => rp.Product)
                .WithMany(d => d.DispatchProducts)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder
                .Entity<DispatchProduct>()
                .HasOne(rp => rp.Dispatch)
                .WithMany(d => d.DispatchProducts)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    d => DateOnly.FromDateTime(d))
            { }
        }

        public class DateOnlyNullableConverter : ValueConverter<DateOnly?, DateTime?>
        {
            public DateOnlyNullableConverter() : base(
                    d => d == null ? null : ((DateOnly)d).ToDateTime(TimeOnly.MinValue),
                    d => d == null ? null : DateOnly.FromDateTime((DateTime)d))
            { }
        }
    }
}
