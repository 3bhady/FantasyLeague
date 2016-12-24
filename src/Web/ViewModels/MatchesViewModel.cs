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
        public MatchesViewModel()
        {
            HomeID = 0;
            AwayID = 0;
            ValidBetAccept =true;
            CanBet = true;
            HaveBetHistory = false;
        }
        public int HomeID { set; get; }
        public int AwayID { set; get; }
        public int BettedTeamID { set; get; }
        public int BettedValue { set; get; }
        public int UserPoints { set; get; }
        public List<SelectListItem> RoundMatches { set; get; }
        public int UpdatedMatchID  { get; set; }
        public int UpdatedMatchHomeTeamScore { get; set; }
        public int UpdatedMatchAwayTeamScore { get; set; }
        public bool ValidBetAccept { get; set; }
        public bool CanBet { get; set; }
        public List<object[]> ResultList { get; set; }
        public bool HaveBetHistory { get; set; }
    }
}
