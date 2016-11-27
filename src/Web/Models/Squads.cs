using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public partial class Squads
    {
        public Squads()
        {
            SquadsPlayersLineup = new HashSet<SquadsPlayersLineup>();
            SquadsPlayersTemp = new HashSet<SquadsPlayersTemp>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SquadId { get; set; }
        public int UserId { get; set; }
        public int HeroTeamId { get; set; }
        public int HeroPlayerId { get; set; }
        public int Points { get; set; }
        public double Money { get; set; }
        public string Formation { get; set; }
        public string TempFormation { get; set; }
        public string TeamName { get; set; }

        public virtual ICollection<SquadsPlayersLineup> SquadsPlayersLineup { get; set; }
        public virtual ICollection<SquadsPlayersTemp> SquadsPlayersTemp { get; set; }
        public virtual Players HeroPlayer { get; set; }
        public virtual Teams HeroTeam { get; set; }
        public virtual Users User { get; set; }
    }
}
