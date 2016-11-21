using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class SquadsPlayersLineup
    {
        public int SquadId { get; set; }
        public int PlayerId { get; set; }
        public int Round { get; set; }

        public virtual Players Player { get; set; }
        public virtual Squads Squad { get; set; }
    }
}
