using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.ViewModels
{
    public class CompetitionsViewModel
    {
        public List<Competitions> userCompetitions;
        // create competition:
        public Competitions newCompetition;
        public CompetitionsViewModel()
        {
            newCompetition = new Competitions();
        }


    }
}
