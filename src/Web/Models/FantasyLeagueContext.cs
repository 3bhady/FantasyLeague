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
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("USERS");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("varchar(15)");
            });
        }

        public virtual DbSet<Users> Users { get; set; }
    }
}