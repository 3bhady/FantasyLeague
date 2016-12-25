using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.ViewModels
{
    public class SwapPlayers 
    {
        public string primary { get; set; }
        public string secondary { get; set; }
        public SwapPlayers()
        {

        }
    }
}
