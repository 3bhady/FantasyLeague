using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web.Models;

namespace Web.Models
{
    public partial class FantasyLeagueContext : DbContext
    {
        public FantasyLeagueContext(DbContextOptions<FantasyLeagueContext> options)
            : base(options)
        { }

        /*  ***thanks   23/10/2016 ******
         el model builder hena bbsata bst5dmo 3ashan lw 3mlt  database 
         men el models f hena b2ol kol entity htb2a table ezay 
         w a el properties bta3et el tables di 
         bs hena mlhash lzma awi l2nyna 3mlen reverse engineering w 3amlen el
         models men el database
         ////////////////////////////////
         notice to 3bhady:::htla2y el database mech sh8ala l2n el files mech 3ndk
         w samir zwd el teams wl matches bs astna lma ykmlo
         /////notice to samir ::kml el matches wl teams w arf3 el query bta3 el tables kol table lw7do
         f file kol file asmo 3la asm el table
         
           * 
         */
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
            modelBuilder.Entity<Matches>(entity =>
            {
                entity.HasKey(e => e.MatchId)
                    .HasName("PK_Matches");

                entity.Property(e => e.MatchId).ValueGeneratedNever();

                //entity.Property(e => e.MatchTime)
                //    .IsRequired()
                //    .HasColumnType("timestamp")
                //    .ValueGeneratedOnAddOrUpdate();

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

    

        public virtual DbSet<Users> Users { get; set; }

          
        public virtual DbSet<Matches> Matches { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }

    }
}