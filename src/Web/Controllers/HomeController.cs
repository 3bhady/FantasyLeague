using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;
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
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

           
        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

    }
}
