using System;
using DirectorySettlementsDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DirectorySettlementsDAL.Data
{
    /// <summary>
    /// ApplictionContext class is a database context.
    /// </summary>
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        /// <value>InitialTable uses for the initialization.</value>
        public virtual DbSet<InitialTable> InitialTable { get; set; }

        /// <value>Gets acces to the Settlements table.</value>
        public virtual DbSet<Settlement> Settlements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=.\\SqlExpress;Database=directorySettlementsDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InitialTable>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Np)
                    .HasColumnName("NP")
                    .HasMaxLength(255);

                entity.Property(e => e.Nu)
                    .HasColumnName("NU")
                    .HasMaxLength(255);

                entity.Property(e => e.Te)
                    .HasColumnName("TE")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Settlement>(entity =>
            {
                entity.HasKey(e => e.Te);

                entity.Property(e => e.Te)
                    .HasColumnName("TE")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Np)
                    .HasColumnName("NP")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Nu)
                    .HasColumnName("NU")
                    .HasMaxLength(255)
                    .IsFixedLength();

                entity.Property(e => e.ParentId)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Settlements_Settlements");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
