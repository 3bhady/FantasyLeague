using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.DBEntities;
using Microsoft.AspNetCore.Http;
using Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        // define private object holding the database context 
        private FantasyLeagueContext _DbContext;
        private IDBReader dbreader;
        public HomeController(FantasyLeagueContext Db, IDBReader dr)
        {
           
            dbreader = dr;
            _DbContext = Db;
        }
        //get request Index Method accessed by /home/Index 
        [HttpGet]
        public IActionResult Index()
        {
       

            return View();
        }
   


        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Competitions()
        {
            CompetitionsViewModel competitionsViewModel= new CompetitionsViewModel();
      //to do , select all competitons,users participating , points from
      //competitions where user

            return View( competitionsViewModel);
        }
        [HttpPost]
        public IActionResult Competitions(Competitions competition)
        {

            
            return RedirectToAction("Competitions");
        }
    }
}
