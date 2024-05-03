// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Infrasturcture
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="BdSqlBathroomContext.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Bathroom.Domain.Aggregates;
using Juegos.Serios.Bathroom.Domain.Entities;
using Juegos.Serios.Shared.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Juegos.Serios.Bathroom.Infrastructure.Persistence;

public partial class BdSqlBathroomContext : DbContext
{
    public BdSqlBathroomContext()
    {
    }

    public BdSqlBathroomContext(DbContextOptions<BdSqlBathroomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Questionnaire> Questionnaires { get; set; }

    public virtual DbSet<QuestionnaireAnswer> QuestionnaireAnswers { get; set; }

    public virtual DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }

    public virtual DbSet<Weight> Weights { get; set; }


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
        modelBuilder.Entity<Questionnaire>(entity =>
        {
            entity.HasKey(e => e.QuestionnaireId).HasName("PK__Question__A56EF40538EA2687");

            entity.ToTable("Questionnaires", "Bathroom");

            entity.Property(e => e.QuestionnaireId)
                .ValueGeneratedNever()
                .HasColumnName("QuestionnaireID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<QuestionnaireAnswer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__Question__D4825024A6B50257");

            entity.ToTable("QuestionnaireAnswers", "Bathroom");

            entity.Property(e => e.AnswerId)
                .ValueGeneratedNever()
                .HasColumnName("AnswerID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.WeightId).HasColumnName("WeightID");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionnaireAnswers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__Questionn__Quest__22751F6C");

            entity.HasOne(d => d.Weight).WithMany(p => p.QuestionnaireAnswers)
                .HasForeignKey(d => d.WeightId)
                .HasConstraintName("FK__Questionn__Weigh__2180FB33");
        });

        modelBuilder.Entity<QuestionnaireQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06F8CFB90DDC2");

            entity.ToTable("QuestionnaireQuestions", "Bathroom");

            entity.Property(e => e.QuestionId)
                .ValueGeneratedNever()
                .HasColumnName("QuestionID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Question)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.QuestionnaireId).HasColumnName("QuestionnaireID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Questionnaire).WithMany(p => p.QuestionnaireQuestions)
                .HasForeignKey(d => d.QuestionnaireId)
                .HasConstraintName("FK__Questionn__Quest__1EA48E88");
        });

        modelBuilder.Entity<Weight>(entity =>
        {
            entity.HasKey(e => e.WeightId).HasName("PK__Weights__02A0F3FB08A9E020");

            entity.ToTable("Weights", "Bathroom");

            entity.Property(e => e.WeightId)
                .ValueGeneratedNever()
                .HasColumnName("WeightID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Weight1).HasColumnName("Weight");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
