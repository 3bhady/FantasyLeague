using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        FantasyLeagueContext _context;

        public HomeController(FantasyLeagueContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index(Matches Match)
        {
            _context.Matches.Add(Match);
            return View();
        }
    }
}
