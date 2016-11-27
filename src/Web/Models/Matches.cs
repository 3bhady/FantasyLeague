using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public partial class Matches
    {
        public Matches()
        {
            BetsHistory = new HashSet<BetsHistory>();
            BetsRequests = new HashSet<BetsRequests>();
            News = new HashSet<News>();
            PlayersMatchesPlayed = new HashSet<PlayersMatchesPlayed>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MatchId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public int RoundNumber { get; set; }
        public int AdminId { get; set; }
        public DateTime? Date { get; set; }

        public virtual ICollection<BetsHistory> BetsHistory { get; set; }
        public virtual ICollection<BetsRequests> BetsRequests { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<PlayersMatchesPlayed> PlayersMatchesPlayed { get; set; }
        public virtual Admins Admin { get; set; }
        public virtual Teams AwayTeam { get; set; }
        public virtual Teams HomeTeam { get; set; }
    }
}
