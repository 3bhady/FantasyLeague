using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Matches
    {
        public int MatchId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public byte[] MatchTime { get; set; }

        public virtual Teams AwayTeam { get; set; }
        public virtual Teams Match { get; set; }
    }
}
