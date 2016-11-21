using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class BetsHistory
    {
        public int User1Id { get; set; }
        public string BetStatus { get; set; }
        public int MatchId { get; set; }
        public int User2Id { get; set; }
        public int Points { get; set; }

        public virtual Matches Match { get; set; }
        public virtual Users User1 { get; set; }
        public virtual Users User2 { get; set; }
    }
}
