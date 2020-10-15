using Castle.Core.Configuration;
using GDipSA51_Team5.Models;
using Microsoft.EntityFrameworkCore;

namespace GDipSA51_Team5.Data
{
    public class Team5_Db: DbContext
    {
        protected IConfiguration configuration;

        public Team5_Db(DbContextOptions<Team5_Db> options)
            : base(options)
        {
            // options like which database provider to use (e.g.
            // MS SQL, Oracle, SQL Lite, MySQL
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(c => c.UserId);

            modelBuilder.Entity<Product>()
                .HasKey(c => c.ProductId);

            modelBuilder.Entity<Cart>()
                .HasAlternateKey(e => new { e.UserId, e.ProductId });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.SerialNo);
            });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
