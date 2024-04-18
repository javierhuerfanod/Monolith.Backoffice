// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="BdSqlJuegosSeriosContext.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Common;
using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Microsoft.EntityFrameworkCore;

namespace Juegos.Serios.Authenticacions.Infrastructure.Persistence;

public partial class BdSqlAuthenticationContext : DbContext
{
    public BdSqlAuthenticationContext()
    {
    }

    public BdSqlAuthenticationContext(DbContextOptions<BdSqlAuthenticationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DocumentTypeEntity> DocumentTypes { get; set; }

    public virtual DbSet<PasswordRecoveryEntity> PasswordRecoveries { get; set; }

    public virtual DbSet<RolEntity> Roles { get; set; }

    public virtual DbSet<SessionLogEntity> SessionLogs { get; set; }

    public virtual DbSet<UserAggregate> Users { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = 3;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = 3;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentTypeEntity>(entity =>
        {
            entity.HasKey(e => e.DocumentTypeId).HasName("PK__Document__DBA390C13F5FB552");

            entity.ToTable("DocumentTypes", "Authentication");

            entity.HasIndex(e => e.TypeName, "UQ__Document__D4E7DFA8B296CD72").IsUnique();

            entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TypeName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PasswordRecoveryEntity>(entity =>
        {
            entity.HasKey(e => e.RecoveryId).HasName("PK__Password__EE4C844C3A185030");

            entity.ToTable("PasswordRecovery", "Authentication");

            entity.Property(e => e.RecoveryId).HasColumnName("RecoveryID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RecoveryPasswordExpiration).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PasswordRecoveryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__PasswordR__Creat__19DFD96B");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PasswordRecoveryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__PasswordR__Updat__1AD3FDA4");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordRecoveryUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PasswordR__UserI__18EBB532");
        });

        modelBuilder.Entity<RolEntity>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A1A328D66");

            entity.ToTable("Roles", "Authentication");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160F4DE1E5D").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<SessionLogEntity>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__SessionL__5E5499A88726928B");

            entity.ToTable("SessionLogs", "Authentication");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                 .HasDefaultValueSql("(getdate())")
                 .HasColumnType("datetime");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SessionLogCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_SessionLogs_CreatedBy");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SessionLogUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_SessionLogs_UpdatedBy");

            entity.HasOne(d => d.User).WithMany(p => p.SessionLogUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SessionLo__UserI__1F98B2C1");
        });

        modelBuilder.Entity<UserAggregate>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC07E7D606");

            entity.ToTable("Users", "Authentication");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E499FFE0C3").IsUnique();

            entity.HasIndex(e => e.DocumentNumber, "UQ__Users__689939184C980168").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105343696D236").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsTemporaryPassword).HasDefaultValue(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Users__CreatedBy__1332DBDC");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Users)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__DocumentT__123EB7A3");

            entity.HasOne(d => d.Rol).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleID__114A936A");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Users__UpdatedBy__14270015");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
