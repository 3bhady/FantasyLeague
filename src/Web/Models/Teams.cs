using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Teams
    {
        public Teams()
        {
            BetsTeam1 = new HashSet<Bets>();
            BetsTeam2 = new HashSet<Bets>();
            BetsRequests = new HashSet<BetsRequests>();
            MatchesAwayTeam = new HashSet<Matches>();
            MatchesHomeTeam = new HashSet<Matches>();
            News = new HashSet<News>();
            Players = new HashSet<Players>();
            Squads = new HashSet<Squads>();
        }

        public int TeamId { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Bets> BetsTeam1 { get; set; }
        public virtual ICollection<Bets> BetsTeam2 { get; set; }
        public virtual ICollection<BetsRequests> BetsRequests { get; set; }
        public virtual ICollection<Matches> MatchesAwayTeam { get; set; }
        public virtual ICollection<Matches> MatchesHomeTeam { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Players> Players { get; set; }
        public virtual ICollection<Squads> Squads { get; set; }
    }
}
