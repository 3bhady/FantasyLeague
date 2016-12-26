using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels
{
    public class PlayerViewModel
    {
        public List<object[]> Players { get; set; }
        public string PlayerID { get; set; }
    }
}
