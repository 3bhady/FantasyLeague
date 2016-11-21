using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Bets
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public int Points { get; set; }
        public int MatchId { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }

        public virtual Teams Team1 { get; set; }
        public virtual Teams Team2 { get; set; }
        public virtual Users User1 { get; set; }
        public virtual Users User2 { get; set; }
    }
}
