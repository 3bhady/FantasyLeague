using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class PlayersMatchesPlayed
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int AdminId { get; set; }
        public int Goals { get; set; }
        public int Points { get; set; }

        public virtual Admins Admin { get; set; }
        public virtual Matches Match { get; set; }
        public virtual Players Player { get; set; }
    }
}
