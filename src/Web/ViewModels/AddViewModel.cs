﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels
{
    public class AddViewModel
    {
        public Matches Match { get; set; }
        public Players Player { get; set; }
        public Teams Team { get; set; }
        public News NewsPost { get; set; }
        public List<SelectListItem> AllTeams { set; get; }
        public int SelectedTeamID { set; get; }

    }
}
