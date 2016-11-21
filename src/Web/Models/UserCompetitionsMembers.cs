using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class UserCompetitionsMembers
    {
        public int UserId { get; set; }
        public int CompetitionId { get; set; }
        public DateTime JoinedDate { get; set; }

        public virtual Competitions Competition { get; set; }
        public virtual Users User { get; set; }
    }
}
