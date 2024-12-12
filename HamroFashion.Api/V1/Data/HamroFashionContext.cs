using Azure;
using HamroFashion.Api.V1.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Security;

namespace HamroFashion.Api.V1.Data
{
    /// <summary>
    /// Our entity framework database context for hamrofashion models
    /// </summary>
    public class HamroFashionContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductCollection> ProductCollections { get; set; }
        public DbSet<ProductLabel> ProductLabels { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
        public DbSet<CartEntity> CartEntities { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderEnity> Orders {  get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="options">The DbContextOptions to build with</param>
        public HamroFashionContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Overrides so that we can automate some audit fields.
        /// </summary>
        /// <returns>Number of rows affected</returns>
        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        /// <summary>
        /// Overrides so that we can automate some audit fields.
        /// </summary>
        /// <returns>Number of rows affected</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Builds our context
        /// </summary>
        /// <param name="modelBuilder">Model builder to configure</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Sets some common audit fields on all added or modified BaseModels (or derivitives)
        /// </summary>
        protected void SetAuditFields()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity model)
                {
                    if (entry.State == EntityState.Added)
                        model.CreatedOn = DateTime.UtcNow;

                    if (entry.State == EntityState.Modified)
                        model.ModifiedOn = DateTime.UtcNow;
                }
            }
        }

        /// <summary>
        /// For some of the middleware that conflict with minimal API pipeline when using the
        /// scoped AuthContext. (Such as HamroFashionJwtBearerHandler)
        /// </summary>
        public class TransientHamroFashionContext : HamroFashionContext
        {
            /// <inheritdoc />
            public TransientHamroFashionContext(DbContextOptions options) : base(options) { }
        }
    }
}
