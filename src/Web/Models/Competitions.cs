using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Competitions
    {
        public Competitions()
        {
            UserCompetitionsMembers = new HashSet<UserCompetitionsMembers>();
            UserCompetitionsRequests = new HashSet<UserCompetitionsRequests>();
        }

        public int CompetitionId { get; set; }
        public int AdminId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Code { get; set; }

        public virtual ICollection<UserCompetitionsMembers> UserCompetitionsMembers { get; set; }
        public virtual ICollection<UserCompetitionsRequests> UserCompetitionsRequests { get; set; }
        public virtual Users Admin { get; set; }
    }
}
