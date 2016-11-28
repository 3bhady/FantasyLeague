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
using Microsoft.ApplicationInsights.DataContracts;
using Web.Entities;


//using Web.Entities;

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
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("ID") != null)
                return RedirectToAction("Index");

                return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(Users user)
        {
            //Validate
            var Model =
                dbreader.GetData("SELECT user_id from Users where username='"+user.Username+ "' AND password='"+user.Password+"'" ,"List");


             var LoginViewModel = (List<object[]>) Model;



            if (LoginViewModel.Count()!=0)
            {
                HttpContext.Session.SetInt32("ID", user.UserId);
                return RedirectToAction("Index");
            }
            else
                return View("Login");
        }


        //get request Index Method accessed by /home/Index 
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Matches()
        {

            string query = "SELECT T1.name,home_team_score,away_team_score,T2.name" +
             " FROM Matches, Teams AS T1, Teams AS T2" +
             " WHERE round_number IN (SELECT MAX(round_number) FROM Matches) AND home_team_id = T1.team_id AND away_team_id = T2.team_id";

            var Model = dbreader.GetData(query, "List");
            List<object[]> matches = (List < object[] >) Model;

            return View(matches);
        }

        [HttpGet]
        public IActionResult AddMatches()
        {
            if (HttpContext.Session.GetInt32("AdminID") == null)
                HttpContext.Session.SetInt32("AdminID",1);
            return View();
        }

        [HttpPost]
        public IActionResult AddMatches(Matches match)
        {

            string query = "INSERT INTO Matches (home_team_id, away_team_id, round_number, admin_id, date)" +
            " VALUES ('"+match.HomeTeamId+ "','" + match.AwayTeamId + "','" + match.RoundNumber + "','" + HttpContext.Session.GetInt32("AdminID") + "','" + match.Date + "')";

            dbreader.ExecuteNonQuery(query);

            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
   

        [HttpPost]
        public IActionResult Signup(SignupViewModel signupViewModel)
        {
            Users User = signupViewModel.User;
            if (signupViewModel.PasswordAgain != User.Password)
                return View();
            if (User.Email == "")
                return View();

            //check model state validation
          //  if(ModelState != null && ModelState.IsValid)





            //Validate
            var Model = _DbContext.Users.FirstOrDefault(r => r.Username == User.Username);
            if (Model != null)
            {
                return View();
                //  return RedirectToAction("Index", new { id = Model.Id });

            }
            else
            {

                _DbContext.Users.Add(User);
                _DbContext.SaveChanges();
                HttpContext.Session.SetInt32("ID",User.UserId);

                return RedirectToAction("Index");
                
            }
        
           // Users User;
           
            //User = signupViewModel.User;
            //do validation
            //check if it already exist in database

        //    _DbContext.Users.Add(User);
        //    _DbContext.SaveChanges();
           
        }


      
    }
}
