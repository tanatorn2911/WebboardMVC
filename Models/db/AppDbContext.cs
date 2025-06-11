using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebboardMVC.Models.db
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Kratoo> Kratoos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=my_db.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(
                  new { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
                  new { Id = Guid.NewGuid().ToString(), Name = "Member", NormalizedName = "MEMBER" }
              );


            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => new { e.Kid, e.CommentNo })
                    .HasName("PK__Reply__B873363C4B81568C");

                entity.Property(e => e.Kid).HasColumnName("KID");

                entity.Property(e => e.CommentNo)
                    .ValueGeneratedOnAdd() //
                    .HasColumnName("CommentNO");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ReplyDate).HasColumnType("datetime");

                entity.Property(e => e.UserIp)
                    .HasMaxLength(100)
                    .HasColumnName("UserIP");

                //entity.Property(e => e.UserName).HasMaxLength(50);

                //entity.HasOne(d => d.KidNavigation)
                //    .WithMany(p => p.Comments)
                //    .HasForeignKey(d => d.Kid)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Reply_Topic");
            });

            modelBuilder.Entity<Kratoo>(entity =>
            {
                entity.HasKey(e => e.Kid)
                    .HasName("PK__Topic__C456D729EF6C2ECF");

                entity.Property(e => e.Kid).HasColumnName("KID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.KratooDetails).HasMaxLength(255);

                entity.Property(e => e.KratooTopic).HasMaxLength(100);

                entity.Property(e => e.RecordDate).HasColumnType("datetime");

                entity.Property(e => e.UserIp)
                    .HasMaxLength(100)
                    .HasColumnName("UserIP");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Kratoos)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kratoos_Categories");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
