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
        private FantasyLeagueContext _DbContext;
        public HomeController(FantasyLeagueContext Db)
        {
            _DbContext = Db;
        }      
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Users user)
        {  
            _DbContext.Users.Add(user);
            _DbContext.SaveChanges();

            return View(user);
        }
      


    }
}
