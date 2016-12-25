using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public partial class Players
    {
        public Players()
        {
            News = new HashSet<News>();
            PlayersMatchesPlayed = new HashSet<PlayersMatchesPlayed>();
            Squads = new HashSet<Squads>();
            SquadsPlayersLineup = new HashSet<SquadsPlayersLineup>();
            SquadsPlayersTemp = new HashSet<SquadsPlayersTemp>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
        public string Type { get; set; }
        public byte[] Image { get; set; }
        public DateTime BirthDate { get; set; }
        public string Status { get; set; }
        public double Cost { get; set; }
        public int TshirtNumber { get; set; }

        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<PlayersMatchesPlayed> PlayersMatchesPlayed { get; set; }
        public virtual ICollection<Squads> Squads { get; set; }
        public virtual ICollection<SquadsPlayersLineup> SquadsPlayersLineup { get; set; }
        public virtual ICollection<SquadsPlayersTemp> SquadsPlayersTemp { get; set; }
        public virtual Teams Team { get; set; }
    }
}
