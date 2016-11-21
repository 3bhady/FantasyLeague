using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class BetsRequests
    {
        public int User1Id { get; set; }
        public int MatchId { get; set; }
        public int Team1Id { get; set; }
        public int Points { get; set; }

        public virtual Matches Match { get; set; }
        public virtual Teams Team1 { get; set; }
        public virtual Users User1 { get; set; }
    }
}
