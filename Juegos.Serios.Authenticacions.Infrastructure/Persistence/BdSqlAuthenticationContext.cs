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
using Juegos.Serios.Authenticacions.Domain.Entities.City;
using Juegos.Serios.Authenticacions.Domain.Entities.DataConsent;
using Juegos.Serios.Authenticacions.Domain.Entities.DocumentType;
using Juegos.Serios.Authenticacions.Domain.Entities.PasswordRecovery;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Entities.SessionLog;
using Juegos.Serios.Authenticacions.Domain.Entities.UserAvatar;
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

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<PasswordRecovery> PasswordRecoveries { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SessionLog> SessionLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<DataConsent> DataConsents { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<UserAvatarBodyPart> UserAvatarBodyParts { get; set; }


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
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21A96910C1556");

            entity.ToTable("Cities", "Authentication");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<DataConsent>(entity =>
        {
            entity.HasKey(e => e.ConsentId).HasName("PK__DataCons__374AB0A66029D9BC");

            entity.ToTable("DataConsent", "Authentication");

            entity.Property(e => e.ConsentId).HasColumnName("ConsentID");
            entity.Property(e => e.ConsentDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DataConsentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataConsent_CreatedBy");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DataConsentUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_DataConsent_UpdatedBy");

            entity.HasOne(d => d.User).WithMany(p => p.DataConsentUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataConsent_Users");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.DocumentTypeId).HasName("PK__Document__DBA390C1C9C6B8E8");

            entity.ToTable("DocumentTypes", "Authentication");

            entity.HasIndex(e => e.TypeName, "UQ__Document__D4E7DFA8847CDA57").IsUnique();

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

        modelBuilder.Entity<PasswordRecovery>(entity =>
        {
            entity.HasKey(e => e.RecoveryId).HasName("PK__Password__EE4C844CB7BA1221");

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
                .HasConstraintName("FK__PasswordR__Creat__7A672E12");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PasswordRecoveryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__PasswordR__Updat__7B5B524B");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordRecoveryUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PasswordR__UserI__7C4F7684");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A4A6909B5");

            entity.ToTable("Roles", "Authentication");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160BAD60DBE").IsUnique();

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

        modelBuilder.Entity<SessionLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__SessionL__5E5499A8AA79D06E");

            entity.ToTable("SessionLogs", "Authentication");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action)
                .HasMaxLength(255)
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
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACAF9F3526");

            entity.ToTable("Users", "Authentication");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E44C0A6ECD").IsUnique();

            entity.HasIndex(e => e.DocumentNumber, "UQ__Users__68993918D2D35C49").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105344CE22BD7").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.BirthdayDate).HasColumnType("datetime");
            entity.Property(e => e.CityHomeId).HasColumnName("CityHomeID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
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

            entity.HasOne(d => d.CityHome).WithMany(p => p.UserCityHomes)
                .HasForeignKey(d => d.CityHomeId)
                .HasConstraintName("FK_Users_CityHomeID");

            entity.HasOne(d => d.City).WithMany(p => p.UserCities)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Users_CityID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Users__CreatedBy__7D439ABD");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Users)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__DocumentT__7E37BEF6");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleID__7F2BE32F");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Users__UpdatedBy__00200768");
        });

        modelBuilder.Entity<UserAvatarBodyPart>(entity =>
        {
            entity.HasKey(e => e.UserAvatarBodyPartsId).HasName("PK__UserAvat__6647D3E5B873B90D");

            entity.ToTable("UserAvatarBodyParts", "Authentication");

            entity.Property(e => e.BodyPartAnimationId).HasColumnName("BodyPartAnimationID");
            entity.Property(e => e.BodyPartName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserAvatarBodyParts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserAvata__UserI__17F790F9");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
