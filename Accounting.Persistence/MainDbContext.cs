using Accounting.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Persistence
{
    public class MainDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductRecord> ProductRecords { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionDocument> CollectionDocuments { get; set; }
        public DbSet<Corporation> Corporations { get; set; }
        public DbSet<CorporationRecord> CorporationRecords { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<Tenant> Tenant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region User Table and Naming
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<UserToken>().ToTable("UserToken");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaim");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ProductRecord>().ToTable("ProductRecord");
            modelBuilder.Entity<Collection>().ToTable("Collection");
            modelBuilder.Entity<CollectionDocument>().ToTable("CollectionDocument");
            modelBuilder.Entity<Corporation>().ToTable("Corporation");
            modelBuilder.Entity<CorporationRecord>().ToTable("CorporationRecord");
            modelBuilder.Entity<ProductProperty>().ToTable("ProductProperty");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<ProductImage>().ToTable("ProductImage");
            modelBuilder.Entity<UserRole>().HasOne(p => p.User).WithMany(p => p.Roles).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserRole>().HasOne(p => p.Role).WithMany(p => p.Users).HasForeignKey(p => p.RoleId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>().Property(p => p.IsDeleted).HasDefaultValue(false);
            #endregion
            #region Relations
            modelBuilder.Entity<CollectionDocument>().HasOne(p => p.Collection).WithMany(p => p.CollectionDocuments).HasForeignKey(p => p.CollectionId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CorporationRecord>().HasOne(p => p.Corporation).WithMany(p => p.CorporationRecord).HasForeignKey(p => p.CorporationId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductRecord>().HasOne(p => p.Product).WithMany(p => p.ProductRecord).HasForeignKey(p => p.ProductId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductImage>().HasOne(p => p.Product).WithMany(p => p.Image).HasForeignKey(p => p.ProductId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductProperty>().HasOne(p => p.Product).WithMany(p => p.Properties).HasForeignKey(p => p.ProductId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Order>().HasOne(p=>p.Corporation).WithMany(p=>p.Order).HasForeignKey(p=>p.CorporationId).IsRequired(true).OnDelete(DeleteBehavior.NoAction);
            #endregion
        }
    }
}


