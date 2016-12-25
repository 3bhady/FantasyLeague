using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Squad
    {
        public int SquadId { get; set; }
        public int UserId { get; set; }
        [Required]
        public int HeroTeamId { get; set; }
        [Required]
        public int HeroPlayerId { get; set; }
        public int Points { get; set; }
        public double Money { get; set; }
        [Required]
        [MinLength(4)]
        public string Formation { get; set; }
    
        
       
        public string TeamName { get; set; }
    }
}
