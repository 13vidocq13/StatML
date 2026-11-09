using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace StatML.Models;

//public partial class AppDbContext : DbContext
public partial class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Login> Logins { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Artifact> Artifacts { get; set; }

    public DbSet<FarmZone> FarmZones { get; set; }

    public DbSet<Grail> Grails { get; set; }

    public DbSet<Player> Players { get; set; }

    public DbSet<Promotion> Promotions { get; set; }

    public DbSet<Salt> Salts { get; set; }

    public DbSet<Sett> Sets { get; set; }

    public DbSet<Tournament> Tournaments { get; set; }

    public DbSet<TournamentType> TournamentTypes { get; set; }

    public DbSet<Utopia> Utopias { get; set; }
    public DbSet<FlagTournamentDetails> FlagTournamentDetails { get; set; }
    public DbSet<ArhiRuin> ArhiRuins { get; set; }
    public DbSet<RatingArmy> RatingArmies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite("YourConnectionString")
            .LogTo(Console.WriteLine, LogLevel.Information);

        optionsBuilder.UseSqlite("Data Source=App_Data/StatML.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasOne(s => s.CreatorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.CreatorId);

            entity.HasOne(s => s.EditorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.EditorId);

           // entity.HasOne(s => s.LoginEntity)
           //.WithMany()
           //.HasForeignKey(s => s.LoginEntity.Id);
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasOne(s => s.PlayerEntity)
            .WithOne(s => s.LoginEntity)
            .HasForeignKey<Login>(s => s.PlayerId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasMany(d => d.LoginEntities)
            .WithOne(p => p.RoleEntity)
            .HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<Artifact>(entity =>
        {
            entity.HasOne(d => d.SetEntity)
            .WithMany(p => p.ArtifactsEntities)
            .HasForeignKey(d => d.SetId);
        });

        modelBuilder.Entity<FarmZone>(entity =>
        {
            entity.HasOne(d => d.PlayerEntity)
            .WithMany(p => p.FarmZonesEntities)
            .HasForeignKey(d => d.PlayerId);
        });

        modelBuilder.Entity<Grail>(entity =>
        {
            entity.HasOne(d => d.PlayerEntity)
            .WithMany(p => p.GrailEntities).
            HasForeignKey(d => d.PlayerId);

            entity.HasOne(s => s.CreatorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.CreatorId);

            entity.HasOne(s => s.EditorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.EditorId);
        });

        modelBuilder.Entity<Salt>(entity =>
        {
            entity.HasOne(d => d.PlayerEntity)
            .WithMany(p => p.SaltEntities).
            HasForeignKey(d => d.PlayerId);

            entity.HasOne(s => s.CreatorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.CreatorId);

            entity.HasOne(s => s.EditorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.EditorId);
        });
        
        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.HasOne(d => d.ArtifactEntity)
            .WithMany(p => p.Tournaments)
            .HasForeignKey(d => d.ArtifactId);

            entity.HasOne(d => d.PlayerEntity)
            .WithMany(p => p.TournamentsEntities)
            .HasForeignKey(d => d.PlayerId);

            entity.HasOne(d => d.TournamentTypeEntity)
            .WithMany(p => p.Tournaments)
            .HasForeignKey(d => d.TournamentTypeId);

            entity.HasOne(s => s.CreatorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.CreatorId);

            entity.HasOne(s => s.EditorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.EditorId);
        });

        modelBuilder.Entity<Utopia>(entity =>
        {
            entity.ToTable("Utopias");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.PlayerId1)
                  .HasColumnName("PlayerId1")
                  .IsRequired();
            entity.Property(u => u.PlayerId2)
                  .HasColumnName("PlayerId2")
                  .IsRequired();
            entity.HasOne(u => u.PlayerEntity1)
                  .WithMany(p => p.UtopiaPlayerId1)
                  .HasForeignKey(u => u.PlayerId1)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(u => u.PlayerEntity2)
                  .WithMany(p => p.UtopiaPlayerId2)
                  .HasForeignKey(u => u.PlayerId2)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.CreatorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.CreatorId);

            entity.HasOne(s => s.EditorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.EditorId);
        });

        modelBuilder.Entity<FlagTournamentDetails>(entity =>
        {
            entity.ToTable("FlagTournamentDetails");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.PlayerId)
                  .HasColumnName("PlayerId") 
                  .IsRequired();
            entity.HasOne(d => d.PlayerEntity)
                  .WithMany(p => p.FlagTournamentDetailsEntities)
                  .HasForeignKey(d => d.PlayerId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ArhiRuin>(entity =>
        {
            entity.HasOne(d => d.PlayerEntity)
            .WithMany(p => p.ArhiRuinEntities).
            HasForeignKey(d => d.PlayerId);

            entity.HasOne(s => s.CreatorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.CreatorId);

            entity.HasOne(s => s.EditorLoginEntity)
            .WithMany()
            .HasForeignKey(s => s.EditorId);
        });

        modelBuilder.Entity<RatingArmy>(entity =>
        {
            entity.HasOne(d => d.PlayerEntity)
            .WithMany(x => x.RatingArmyEntities)
            .HasForeignKey(x => x.PlayerId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
