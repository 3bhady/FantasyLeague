using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Web.Models
{
    public partial class FantasyLeagueContext : DbContext
    {
        public FantasyLeagueContext(DbContextOptions<FantasyLeagueContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Matches>(entity =>
            {
                entity.HasKey(e => e.MatchId)
                    .HasName("PK_Matches");

                entity.Property(e => e.MatchId).ValueGeneratedNever();

                entity.Property(e => e.MatchTime)
                    .IsRequired()
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.AwayTeam)
                    .WithMany(p => p.MatchesAwayTeam)
                    .HasForeignKey(d => d.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Matches_Teams");

                entity.HasOne(d => d.Match)
                    .WithOne(p => p.MatchesMatch)
                    .HasForeignKey<Matches>(d => d.MatchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Matches_Teams1");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.HasKey(e => e.TeamId)
                    .HasName("PK_Teams");

                entity.Property(e => e.TeamId).ValueGeneratedNever();

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasColumnType("nchar(20)");
            });
        }

        public virtual DbSet<Matches> Matches { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
    }
}