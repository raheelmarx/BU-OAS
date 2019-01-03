using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OfficeAuto.Models.DB
{
    public partial class BUOASContext : DbContext
    {
        public BUOASContext()
        {
        }

        public BUOASContext(DbContextOptions<BUOASContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Campuses> Campuses { get; set; }
        public virtual DbSet<Consultants> Consultants { get; set; }
        public virtual DbSet<Contractors> Contractors { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<UserAolAod> UserAolAod { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-BIO4H1C\\SQLEXPRESS;Database=BUOAS;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Campuses>(entity =>
            {
                entity.Property(e => e.CampusCode).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Consultants>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(10);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(100);
            });

            modelBuilder.Entity<Contractors>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(10);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(100);
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.Property(e => e.DeptCode).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<UserAolAod>(entity =>
            {
                entity.ToTable("User_Aol_Aod");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AoUserid)
                    .HasColumnName("ao_userid")
                    .HasMaxLength(100);

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("date");

                entity.Property(e => e.DateAssigned)
                    .HasColumnName("date_assigned")
                    .HasColumnType("date");

                entity.Property(e => e.DateExpired)
                    .HasColumnName("date_expired")
                    .HasColumnType("date");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(50);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(100);
            });
        }
    }
}
