using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Web.Models
{
    public partial class FantasyLeagueContext : DbContext
    {
        public FantasyLeagueContext(DbContextOptions<FantasyLeagueContext> options)
        : base(options) { }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>(entity =>
            {
                entity.HasKey(e => e.AdminId)
                    .HasName("PK_Admins");

                entity.HasIndex(e => e.Email)
                    .HasName("IX_Admins_1")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("IX_Admins")
                    .IsUnique();

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Bets>(entity =>
            {
                entity.HasKey(e => new { e.User1Id, e.MatchId })
                    .HasName("PK_Bets_1");

                entity.Property(e => e.User1Id).HasColumnName("user1_id");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.Points).HasColumnName("points");

                entity.Property(e => e.Team1Id).HasColumnName("team1_id");

                entity.Property(e => e.Team2Id).HasColumnName("team2_id");

                entity.Property(e => e.User2Id).HasColumnName("user2_id");

                entity.HasOne(d => d.Team1)
                    .WithMany(p => p.BetsTeam1)
                    .HasForeignKey(d => d.Team1Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_Teams");

                entity.HasOne(d => d.Team2)
                    .WithMany(p => p.BetsTeam2)
                    .HasForeignKey(d => d.Team2Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_Teams1");

                entity.HasOne(d => d.User1)
                    .WithMany(p => p.BetsUser1)
                    .HasForeignKey(d => d.User1Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_Users");

                entity.HasOne(d => d.User2)
                    .WithMany(p => p.BetsUser2)
                    .HasForeignKey(d => d.User2Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_Users1");
            });

            modelBuilder.Entity<BetsHistory>(entity =>
            {
                entity.HasKey(e => new { e.User1Id, e.MatchId })
                    .HasName("PK_Bets_History");

                entity.ToTable("Bets_History");

                entity.Property(e => e.User1Id).HasColumnName("user1_id");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.BetStatus)
                    .IsRequired()
                    .HasColumnName("bet_status")
                    .HasMaxLength(50);

                entity.Property(e => e.Points).HasColumnName("points");

                entity.Property(e => e.User2Id).HasColumnName("user2_id");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.BetsHistory)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_History_Matches");

                entity.HasOne(d => d.User1)
                    .WithMany(p => p.BetsHistoryUser1)
                    .HasForeignKey(d => d.User1Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_History_Users");

                entity.HasOne(d => d.User2)
                    .WithMany(p => p.BetsHistoryUser2)
                    .HasForeignKey(d => d.User2Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_History_Users1");
            });

            modelBuilder.Entity<BetsRequests>(entity =>
            {
                entity.HasKey(e => new { e.User1Id, e.MatchId })
                    .HasName("PK_Bets_Requests");

                entity.ToTable("Bets_Requests");

                entity.Property(e => e.User1Id).HasColumnName("user1_id");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.Points).HasColumnName("points");

                entity.Property(e => e.Team1Id).HasColumnName("team1_id");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.BetsRequests)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_Requests_Matches");

                entity.HasOne(d => d.Team1)
                    .WithMany(p => p.BetsRequests)
                    .HasForeignKey(d => d.Team1Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_Requests_Teams");

                entity.HasOne(d => d.User1)
                    .WithMany(p => p.BetsRequests)
                    .HasForeignKey(d => d.User1Id)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bets_Requests_Users");
            });

            modelBuilder.Entity<Competitions>(entity =>
            {
                entity.HasKey(e => e.CompetitionId)
                    .HasName("PK_Competitions");

                entity.HasIndex(e => e.Code)
                    .HasName("IX_Competitions")
                    .IsUnique();

                entity.Property(e => e.CompetitionId)
                    .HasColumnName("competition_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Competitions)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Competitions_Users");
            });

            modelBuilder.Entity<Matches>(entity =>
            {
                entity.HasKey(e => e.MatchId)
                    .HasName("PK_Matches");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.AwayTeamId).HasColumnName("away_team_id");

                entity.Property(e => e.AwayTeamScore).HasColumnName("away_team_score");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.HomeTeamId).HasColumnName("home_team_id");

                entity.Property(e => e.HomeTeamScore).HasColumnName("home_team_score");

                entity.Property(e => e.RoundNumber).HasColumnName("round_number");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Matches)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Matches_Admins");

                entity.HasOne(d => d.AwayTeam)
                    .WithMany(p => p.MatchesAwayTeam)
                    .HasForeignKey(d => d.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Matches_Teams1");

                entity.HasOne(d => d.HomeTeam)
                    .WithMany(p => p.MatchesHomeTeam)
                    .HasForeignKey(d => d.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Matches_Teams");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.NewsId)
                    .HasColumnName("news_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasColumnName("author")
                    .HasMaxLength(50);

                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasColumnName("body")
                    .HasColumnType("text");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("text");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_News_Admins");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.MatchId)
                    .HasConstraintName("FK_News_Matches");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_News_Players");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_News_Teams");
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("PK_Players");

                entity.HasIndex(e => e.Name)
                    .HasName("IX_Players")
                    .IsUnique();

                entity.Property(e => e.PlayerId)
                    .HasColumnName("player_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BirthDate)
                    .HasColumnName("birth_date")
                    .HasColumnType("date");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasColumnType("image");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(50);

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Players_Teams");
            });

            modelBuilder.Entity<PlayersMatchesPlayed>(entity =>
            {
                entity.HasKey(e => new { e.MatchId, e.PlayerId })
                    .HasName("PK_Players_Matches_Played");

                entity.ToTable("Players_Matches_Played");

                entity.Property(e => e.MatchId).HasColumnName("match_id");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.Goals)
                    .HasColumnName("goals")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Points)
                    .HasColumnName("points")
                    .HasDefaultValueSql("0");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.PlayersMatchesPlayed)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Players_Matches_Played_Admins");

                entity.HasOne(d => d.Match)
                    .WithMany(p => p.PlayersMatchesPlayed)
                    .HasForeignKey(d => d.MatchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Players_Matches_Played_Matches");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayersMatchesPlayed)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Players_Matches_Played_Players");
            });

            modelBuilder.Entity<Squads>(entity =>
            {
                entity.HasKey(e => e.SquadId)
                    .HasName("PK_Squads");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_Squads")
                    .IsUnique();

                entity.Property(e => e.SquadId)
                    .HasColumnName("squad_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Formation)
                    .IsRequired()
                    .HasColumnName("formation")
                    .HasMaxLength(50);

                entity.Property(e => e.HeroPlayerId).HasColumnName("hero_player_id");

                entity.Property(e => e.HeroTeamId).HasColumnName("hero_team_id");

                entity.Property(e => e.Money).HasColumnName("money");

                entity.Property(e => e.Points).HasColumnName("points");

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasColumnName("team_name")
                    .HasMaxLength(20);

                entity.Property(e => e.TempFormation)
                    .IsRequired()
                    .HasColumnName("temp_formation")
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.HeroPlayer)
                    .WithMany(p => p.Squads)
                    .HasForeignKey(d => d.HeroPlayerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Squads_Players");

                entity.HasOne(d => d.HeroTeam)
                    .WithMany(p => p.Squads)
                    .HasForeignKey(d => d.HeroTeamId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Squads_Teams");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Squads)
                    .HasForeignKey<Squads>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Squads_Users");
            });

            modelBuilder.Entity<SquadsPlayersLineup>(entity =>
            {
                entity.HasKey(e => new { e.SquadId, e.PlayerId, e.Round })
                    .HasName("PK_Squads_Players_Lineup");

                entity.ToTable("Squads_Players_Lineup");

                entity.Property(e => e.SquadId).HasColumnName("squad_id");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.Round).HasColumnName("round");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.SquadsPlayersLineup)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Squads_Players_Lineup_Players");

                entity.HasOne(d => d.Squad)
                    .WithMany(p => p.SquadsPlayersLineup)
                    .HasForeignKey(d => d.SquadId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Squads_Players_Lineup_Squads");
            });

            modelBuilder.Entity<SquadsPlayersTemp>(entity =>
            {
                entity.HasKey(e => new { e.SquadId, e.PlayerId })
                    .HasName("PK_Squads_Players_Temp");

                entity.ToTable("Squads_Players_Temp");

                entity.Property(e => e.SquadId).HasColumnName("squad_id");

                entity.Property(e => e.PlayerId).HasColumnName("player_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.SquadsPlayersTemp)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Squads_Plyaers_Temp_Players");

                entity.HasOne(d => d.Squad)
                    .WithMany(p => p.SquadsPlayersTemp)
                    .HasForeignKey(d => d.SquadId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Squads_Plyaers_Temp_Squads");
            });

            modelBuilder.Entity<Teams>(entity =>
            {
                entity.HasKey(e => e.TeamId)
                    .HasName("PK_Teams");

                entity.HasIndex(e => e.Name)
                    .HasName("IX_Teams")
                    .IsUnique();

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasColumnType("image");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserCompetitionsMembers>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CompetitionId })
                    .HasName("PK_User_Competitions_Members");

                entity.ToTable("User_Competitions_Members");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CompetitionId).HasColumnName("competition_id");

                entity.Property(e => e.JoinedDate)
                    .HasColumnName("joined_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.UserCompetitionsMembers)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_User_Competitions_Members_Competitions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserCompetitionsMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_User_Competitions_Members_Users");
            });

            modelBuilder.Entity<UserCompetitionsRequests>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CompetitionId })
                    .HasName("PK_User_Competitions_Requests");

                entity.ToTable("User_Competitions_Requests");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CompetitionId).HasColumnName("competition_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Competition)
                    .WithMany(p => p.UserCompetitionsRequests)
                    .HasForeignKey(d => d.CompetitionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_User_Competitions_Requests_Competitions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserCompetitionsRequests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_User_Competitions_Requests_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_Users");

           

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(60);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

               

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50);
            });
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<Bets> Bets { get; set; }
        public virtual DbSet<BetsHistory> BetsHistory { get; set; }
        public virtual DbSet<BetsRequests> BetsRequests { get; set; }
        public virtual DbSet<Competitions> Competitions { get; set; }
        public virtual DbSet<Matches> Matches { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<PlayersMatchesPlayed> PlayersMatchesPlayed { get; set; }
        public virtual DbSet<Squads> Squads { get; set; }
        public virtual DbSet<SquadsPlayersLineup> SquadsPlayersLineup { get; set; }
        public virtual DbSet<SquadsPlayersTemp> SquadsPlayersTemp { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<UserCompetitionsMembers> UserCompetitionsMembers { get; set; }
        public virtual DbSet<UserCompetitionsRequests> UserCompetitionsRequests { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        // Unable to generate entity type for table 'dbo.Points_Manager'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Users_Matches_Points'. Please see the warning messages.
    }
}