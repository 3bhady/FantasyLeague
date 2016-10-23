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
        // define private object holding the database context 
        private FantasyLeagueContext _DbContext;
        public HomeController(FantasyLeagueContext Db)
        {
            _DbContext = Db;
        }      
        //get request Index Method accessed by /home/Index 
        public IActionResult Index()
        {
            return View();
        }
        //post request accessed by form where it sends a model of users
        // the Index method get that model and process it then it determine
        //whether it is valid for adding to our db context or discarding it
        [HttpPost]
        public IActionResult Index(Users user)
        {  
            _DbContext.Users.Add(user);
            _DbContext.SaveChanges();

            return View(user);
        }
      


    }
}
