using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels
{
    public class MatchesViewModel 
    {
        public List<SelectListItem> RoundMatches { set; get; }
        public int UpdatedMatchID  { get; set; }
        public int UpdatedMatchHomeTeamScore { get; set; }
        public int UpdatedMatchAwayTeamScore { get; set; }
    }
}
