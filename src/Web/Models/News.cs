using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class News
    {
        public int NewsId { get; set; }
        public DateTime Date { get; set; }
        public int AdminId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public int? MatchId { get; set; }
        public int? TeamId { get; set; }
        public int? PlayerId { get; set; }

        public virtual Admins Admin { get; set; }
        public virtual Matches Match { get; set; }
        public virtual Players Player { get; set; }
        public virtual Teams Team { get; set; }
    }
}
