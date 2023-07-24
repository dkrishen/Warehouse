using Microsoft.EntityFrameworkCore;
using WarehouseSimulation.Models.DatabaseModels;

namespace WarehouseSimulation.Models.Data;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DeliveriesProduct> DeliveriesProducts { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<Dispatch> Dispatches { get; set; }

    public virtual DbSet<DispatchesProduct> DispatchesProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Rack> Racks { get; set; }

    public virtual DbSet<RacksProduct> RacksProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=WAREHOUSE-DATABASE;TrustServerCertificate=True;Trusted_Connection=True;");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeliveriesProduct>(entity =>
        {
            entity.HasKey(e => new { e.DeliveryId, e.ProductId });

            entity.HasIndex(e => e.ProductId, "IX_DeliveriesProducts_ProductId");

            entity.HasOne(d => d.Delivery).WithMany(p => p.DeliveriesProducts)
                .HasForeignKey(d => d.DeliveryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeliveriesProducts_DeliveriesWindow_DeliveryId");

            entity.HasOne(d => d.Product).WithMany(p => p.DeliveriesProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DeliveriesWindow");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Dispatch>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<DispatchesProduct>(entity =>
        {
            entity.HasKey(e => new { e.DispatchId, e.ProductId });

            entity.HasIndex(e => e.ProductId, "IX_DispatchesProducts_ProductId");

            entity.HasOne(d => d.Dispatch).WithMany(p => p.DispatchesProducts)
                .HasForeignKey(d => d.DispatchId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.DispatchesProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Type).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_ProductTypes");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Rack>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Type).WithMany(p => p.Racks)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Racks_ProductTypes");
        });

        modelBuilder.Entity<RacksProduct>(entity =>
        {
            entity.HasKey(e => new { e.RackId, e.ProductId });

            entity.HasIndex(e => e.ProductId, "IX_RacksProducts_ProductId");

            entity.HasOne(d => d.Product).WithMany(p => p.RacksProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Rack).WithMany(p => p.RacksProducts)
                .HasForeignKey(d => d.RackId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
