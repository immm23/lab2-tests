using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace OOPLab1.Models;
#region Attribues
[ExcludeFromCodeCoverage]
#endregion
public partial class PillsContext : DbContext
{
    public PillsContext()
    {
    }

    public PillsContext(DbContextOptions<PillsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PillClass> PillClasses { get; set; }

    public virtual DbSet<Ilness> Ilnesses { get; set; }

    public virtual DbSet<Pharmasy> Pharmasies { get; set; }

    public virtual DbSet<Pill> Pills { get; set; }

  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PillClass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PillClass");

            entity.ToTable("Class");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Ilness>(entity =>
        {
            entity.ToTable("Ilness");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Symptoms).HasMaxLength(10);

            entity.HasMany(d => d.Pills).WithMany(p => p.Illnes)
                .UsingEntity<Dictionary<string, object>>(
                    "PillsAndIlness",
                    r => r.HasOne<Pill>().WithMany()
                        .HasForeignKey("PillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PillsAndIlnesses_Pills"),
                    l => l.HasOne<Ilness>().WithMany()
                        .HasForeignKey("IllnesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PillsAndIlnesses_Ilness"),
                    j =>
                    {
                        j.HasKey("IllnesId", "PillId");
                        j.ToTable("PillsAndIlnesses");
                    });
        });

        modelBuilder.Entity<Pharmasy>(entity =>
        {
            entity.ToTable("Pharmasy");

            entity.Property(e => e.Adress).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OwnerName).HasMaxLength(10);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasMany(d => d.Pills).WithMany(p => p.Pharmasies)
                .UsingEntity<Dictionary<string, object>>(
                    "PillsAndPharmasy",
                    r => r.HasOne<Pill>().WithMany()
                        .HasForeignKey("PillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PillsAndPharmasies_Pills"),
                    l => l.HasOne<Pharmasy>().WithMany()
                        .HasForeignKey("PharmasyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PillsAndPharmasies_Pharmasy"),
                    j =>
                    {
                        j.HasKey("PharmasyId", "PillId");
                        j.ToTable("PillsAndPharmasies");
                    });
        });

        modelBuilder.Entity<Pill>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.ClassNavigation).WithMany(p => p.Pills)
                .HasForeignKey(d => d.Class)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pills_PillClass");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
