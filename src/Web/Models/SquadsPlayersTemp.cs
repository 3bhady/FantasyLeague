using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class SquadsPlayersTemp
    {
        public int SquadId { get; set; }
        public int PlayerId { get; set; }
        public int Status { get; set; }

        public virtual Players Player { get; set; }
        public virtual Squads Squad { get; set; }
    }
}
