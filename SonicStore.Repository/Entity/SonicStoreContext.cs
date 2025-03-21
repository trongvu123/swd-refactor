using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SonicStore.Repository.Entity;

public partial class SonicStoreContext : DbContext
{
    public SonicStoreContext()
    {
    }

    public SonicStoreContext(DbContextOptions<SonicStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }


    public virtual DbSet<Checkout> Orders { get; set; }

    public virtual DbSet<Cart> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }


    public virtual DbSet<ProductImage> ProductImages { get; set; }


    public virtual DbSet<Role> Roles { get; set; }


    public virtual DbSet<StatusOrder> StatusOrders { get; set; }
    public virtual DbSet<StatusPayment> StatusPayments { get; set; }
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }
    public virtual DbSet<Inventory> Storages { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Promotion> Promotion { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        var configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("SonicStore"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83F1227836C");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3213E83F07E0250E");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F4E6EE1EC");

        });



        modelBuilder.Entity<Checkout>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83FCBD4A6FA");

            entity.HasOne(o => o.OrderDetails)
    .WithMany(od => od.Orders)
    .HasForeignKey(o => o.CartId)
    .HasPrincipalKey(od => od.Id);

            entity.HasOne(d => d.Payment).WithOne(p => p.Order)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__payment_i__62E4AA3C");

        });

        modelBuilder.Entity<Cart>(entity =>
        {



            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasKey(od => new { od.StorageId, od.CustomerId, od.AddressId, od.Id });
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3213E83F31074084");
        });
        modelBuilder.Entity<StatusPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__status_payment__3213E83F");
        });
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3213E83FD41103F3");

            entity.HasOne(d => d.BrandNavigation).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__brand_i__4924D839");
            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull);


        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product___3213E83F88F164DB");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_I__produ__4CF5691D");
        });

        modelBuilder.Entity<Promotion>()
                .HasOne(p => p.CreatedByUser)
                .WithMany(u => u.CreatedPromotions)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade); // OK to cascade delete from creator

        modelBuilder.Entity<Promotion>()
            .HasOne(p => p.UpdatedByUser)
            .WithMany(u => u.UpdatedPromotions)
            .HasForeignKey(p => p.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction); // Prevent cascade from updater
        modelBuilder.Entity<Promotion>()
            .Property(p => p.MinimumPurchase)
            .HasPrecision(10, 2); // 10 total digits, 2 after decimal (e.g., 12345678.90)
        // Optionally, configure User self-reference
        modelBuilder.Entity<User>()
            .HasOne(u => u.Account)
            .WithOne(a => a.User)
            .HasForeignKey<User>(u => u.AccountId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83F5E85A4EC");
        });


        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3213E83F97D90152");
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83FD23B5FEA");

            entity.HasOne(d => d.Account).WithOne(p => p.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__account_id__3DB3258D");


        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User_Add__3213E83F371BAF55");

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User_Addr__user___4183B671");
        });
        modelBuilder.Entity<Promotion>()
                .HasOne(p => p.CreatedByUser)
                .WithMany(u => u.CreatedPromotions)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for creator

        // User -> Promotion (UpdatedBy)
        modelBuilder.Entity<Promotion>()
            .HasOne(p => p.UpdatedByUser)
            .WithMany(u => u.UpdatedPromotions)
            .HasForeignKey(p => p.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction); // No cascade for updater

        // User self-reference (UpdateBy)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Account)
            .WithOne(a => a.User)
            .HasForeignKey<User>(u => u.AccountId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        // Apply unique constraints from User entity
        modelBuilder.Entity<User>()
            .HasIndex(u => u.AccountId)
            .IsUnique()
            .HasDatabaseName("UQ__User__46A222CC47970D57");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("UQ__User__AB6E6164D8A3A270");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Phone)
            .IsUnique()
            .HasDatabaseName("UQ__User__B43B145F81F86A8C");


        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}