using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.ViewModels
{
    public class CompetitionsViewModel
    {
        public List<object[]> userCompetitions { get; set; }
        // create competition:
        public Competitions newCompetition { get; set; }
        public List<Object[]> competitionsRequests { get; set; }
        public CompetitionsViewModel()
        {
            newCompetition = new Competitions();
        }


    }
}
