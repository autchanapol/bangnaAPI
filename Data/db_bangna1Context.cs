using System;
using System.Collections.Generic;
using bangnaAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace bangnaAPI.Data
{
    public partial class db_bangna1Context : DbContext
    {
        public db_bangna1Context()
        {
        }

        public db_bangna1Context(DbContextOptions<db_bangna1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Bed> Beds { get; set; } = null!;
        public virtual DbSet<BedActive> BedActives { get; set; } = null!;
        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<OrderFood> OrderFoods { get; set; } = null!;
        public virtual DbSet<Uc> Ucs { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Ward> Wards { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=Autchanapol\\SQL20;Database=db_bangna1;User Id=sa;Password=password12345;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bed>(entity =>
            {
                entity.ToTable("bed");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(255)
                    .HasColumnName("remarks");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.Property(e => e.WardId).HasColumnName("ward_id");
            });

            modelBuilder.Entity<BedActive>(entity =>
            {
                entity.ToTable("bed_active");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BedId).HasColumnName("bed_id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.HnId)
                    .HasMaxLength(50)
                    .HasColumnName("hn_id");

                entity.Property(e => e.HnName)
                    .HasMaxLength(255)
                    .HasColumnName("hn_name");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UdId).HasColumnName("ud_id");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("food");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.FoodName)
                    .HasMaxLength(50)
                    .HasColumnName("food_name");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            modelBuilder.Entity<OrderFood>(entity =>
            {
                entity.ToTable("orderFood");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BedActiveId).HasColumnName("bedActive_id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(255)
                    .HasColumnName("remarks");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            modelBuilder.Entity<Uc>(entity =>
            {
                entity.ToTable("uc");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(50)
                    .HasColumnName("remarks");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.ToTable("ward");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(50)
                    .HasColumnName("remarks");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.Property(e => e.WardName)
                    .HasMaxLength(50)
                    .HasColumnName("ward_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
