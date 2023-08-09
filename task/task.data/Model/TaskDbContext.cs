using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using task.data.Essentials;

namespace task.data.Model
{
    public partial class TaskDbContext : DbContext
    {
        internal bool IsLazyLoading = false;
        public TaskDbContext(bool IsLazyLoading = false)
        {
            this.IsLazyLoading = IsLazyLoading;
        }
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies(true).UseSqlServer(GlobalVariable.ConnectionString);
            }
        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Sellers> Sellers { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Bids> Bids { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Books>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.Title).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Author).HasMaxLength(50).IsUnicode(false);

                entity.Property(e => e.SellerId);

                entity.HasOne(d => d.Sellers).WithMany(p => p.Books)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Books_Sellers");
            });

            modelBuilder.Entity<Sellers>(entity =>
            {
                entity.ToTable("Sellers");
                entity.HasKey(e => e.SellerId);
                entity.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Location).HasMaxLength(100).IsUnicode(false);

            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.Username).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(100).IsUnicode(false);

            });

            modelBuilder.Entity<Bids>(entity =>
            {
                entity.ToTable("Bids");
                entity.HasKey(e => e.BidId);
                entity.Property(e => e.SellerId);
                entity.Property(e => e.BookId);

                entity.HasOne(d => d.Sellers).WithMany(p => p.Bids)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bids_Sellers");

                entity.HasOne(d => d.Books).WithMany(p => p.Bids)
                  .HasForeignKey(d => d.BookId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Bids_Books");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}