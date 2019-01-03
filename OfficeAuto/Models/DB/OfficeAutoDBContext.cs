using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OfficeAuto.Models.DB
{
    public partial class OfficeAutoDBContext : DbContext
    {
        public OfficeAutoDBContext()
        {
        }

        public OfficeAutoDBContext(DbContextOptions<OfficeAutoDBContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<Campuses> Campuses { get; set; }
        public virtual DbSet<Case> Case { get; set; }
        public virtual DbSet<Consultants> Consultants { get; set; }
        public virtual DbSet<Contractors> Contractors { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Minutes> Minutes { get; set; }
        public virtual DbSet<MinutesAssignedDraft> MinutesAssignedDraft { get; set; }
        public virtual DbSet<MinutesAssignedRelease> MinutesAssignedRelease { get; set; }
        public virtual DbSet<MinutesHistory> MinutesHistory { get; set; }
        public virtual DbSet<ReferenceDoc> ReferenceDoc { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-0VVCHT6\\RAHEEL;Database=OfficeAutoDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campuses>(entity =>
            {
                entity.Property(e => e.CampusCode).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Case>(entity =>
            {
                entity.Property(e => e.CaseNumber).HasMaxLength(50);

                entity.Property(e => e.CaseTitle).HasMaxLength(150);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");
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

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.CampusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Departments_Campuses");
            });

            modelBuilder.Entity<Minutes>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.MinuteNumber).HasMaxLength(50);

                entity.Property(e => e.MinuteTitle).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.HasOne(d => d.Case)
                    .WithMany(p => p.Minutes)
                    .HasForeignKey(d => d.CaseId)
                    .HasConstraintName("FK_Minutes_Case");
            });

            modelBuilder.Entity<MinutesAssignedDraft>(entity =>
            {
                entity.Property(e => e.AssignedFromUserId)
                    .HasColumnName("AssignedFrom_UserId")
                    .HasMaxLength(50);

                entity.Property(e => e.AssignedToUserId)
                    .HasColumnName("AssignedTo_UserId")
                    .HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.ResponseReceived).HasColumnName("Response_Received");

                entity.HasOne(d => d.Minute)
                    .WithMany(p => p.MinutesAssignedDraft)
                    .HasForeignKey(d => d.MinuteId)
                    .HasConstraintName("FK_MinutesAssignedDraft_Minutes");
            });

            modelBuilder.Entity<MinutesAssignedRelease>(entity =>
            {
                entity.Property(e => e.AssignedFromUserId)
                    .HasColumnName("AssignedFrom_UserId")
                    .HasMaxLength(50);

                entity.Property(e => e.AssignedToUserId)
                    .HasColumnName("AssignedTo_UserId")
                    .HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.ResponseReceived).HasColumnName("Response_Received");

                entity.HasOne(d => d.Case)
                    .WithMany(p => p.MinutesAssignedRelease)
                    .HasForeignKey(d => d.CaseId)
                    .HasConstraintName("FK_MinutesAssignedRelease_Case");
            });

            modelBuilder.Entity<MinutesHistory>(entity =>
            {
                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.MinuteNumber).HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.HasOne(d => d.Minute)
                    .WithMany(p => p.MinutesHistory)
                    .HasForeignKey(d => d.MinuteId)
                    .HasConstraintName("FK_MinutesHistory_Minutes");
            });

            modelBuilder.Entity<ReferenceDoc>(entity =>
            {
                entity.Property(e => e.Access).HasMaxLength(50);

                entity.Property(e => e.AddedBy).HasMaxLength(50);

                entity.Property(e => e.ContentType).HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DocPath).HasMaxLength(500);

                entity.Property(e => e.Flag).HasMaxLength(50);

                entity.Property(e => e.RefFile).HasColumnType("image");

                entity.Property(e => e.RefTitle).HasMaxLength(150);

                entity.HasOne(d => d.Case)
                    .WithMany(p => p.ReferenceDoc)
                    .HasForeignKey(d => d.CaseId)
                    .HasConstraintName("FK_ReferenceDoc_Case");
                entity.HasOne(d => d.Minute)
                    .WithMany(p => p.ReferenceDoc)
                    .HasForeignKey(d => d.MinuteId)
                    .HasConstraintName("FK_ReferenceDoc_ReferenceDoc");
            });
        }
    }
}
