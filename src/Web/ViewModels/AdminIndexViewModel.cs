using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels
{
    public class AdminIndexViewModel
    {
        public List<object[]> Matches { get; set; }
        public List<object[]> Players { get; set; }
    }
}
