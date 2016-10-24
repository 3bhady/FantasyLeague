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
        public IActionResult Matches()
        {
            var Match = _DbContext.Matches.ToList();
            
            return View(Match);
        }
        //post request accessed by form where it sends a model of users
        // the Index method get that model and process it then it determine
        //whether it is valid for adding to our db context or discarding it


      /*  public IActionResult Index()
        {
            return View();
        }
        */
        [HttpGet]
        public IActionResult Index(int id)
        {
            var model = _DbContext.Users.FirstOrDefault(r => r.Id == id);
            return View(model);
        }


        [HttpPost]
        public IActionResult Index(Users user)
        {
            _DbContext.Users.Add(user);
            _DbContext.SaveChanges();
            return RedirectToAction("Matches");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Users user)
        {
            //Validate
            var Model = _DbContext.Users.FirstOrDefault(r => r.Username == user.Username && r.Password == user.Password);
            if (Model != null)
            {

                return RedirectToAction("Index", new { id = Model.Id });

            }
            else
                return View("Login");
        }

    }
}
