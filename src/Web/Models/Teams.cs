using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Teams
    {
        public Teams()
        {
            MatchesAwayTeam = new HashSet<Matches>();
        }

        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public virtual ICollection<Matches> MatchesAwayTeam { get; set; }
        public virtual Matches MatchesMatch { get; set; }
    }
}
