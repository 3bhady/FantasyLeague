using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.ViewModels
{
    public class SquadViewModel
    {
        public List<object[]> squad { get; set; }
        public Squad userSquad{ get; set;}
        public SquadViewModel()
        {
            userSquad = new Squad();
        }
    }
}
