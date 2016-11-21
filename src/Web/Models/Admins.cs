using System;
using System.Collections.Generic;

namespace Web.Models
{
    public partial class Admins
    {
        public Admins()
        {
            Matches = new HashSet<Matches>();
            News = new HashSet<News>();
            PlayersMatchesPlayed = new HashSet<PlayersMatchesPlayed>();
        }

        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Matches> Matches { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<PlayersMatchesPlayed> PlayersMatchesPlayed { get; set; }
    }
}
