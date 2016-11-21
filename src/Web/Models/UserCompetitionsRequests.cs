using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class UserCompetitionsRequests
    {
        public int UserId { get; set; }
        public int CompetitionId { get; set; }
        public string Status { get; set; }

        public virtual Competitions Competition { get; set; }
        public virtual Users User { get; set; }
    }
}
