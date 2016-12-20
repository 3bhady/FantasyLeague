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

using Microsoft.ApplicationInsights.DataContracts;
using Web.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;



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
            if (HttpContext.Session?.GetInt32("ID") != null)
                return RedirectToAction("Index");

                return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session?.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Login(Users user)
        {
            //Validate
            var Model =
                dbreader.GetData("SELECT user_id from Users where username='"+user.Username+ "' AND password='"+user.Password+"'" ,"List");


             var LoginViewModel = (List<object[]>) Model;



            if (LoginViewModel != null && LoginViewModel.Count()!=0)
            {
                HttpContext.Session.SetInt32("ID", (int)LoginViewModel[0][0]);
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
        public IActionResult InsertMatchResult()
        {
            // if (HttpContext.Session?.GetInt32("AdminID") != null)
            //    return RedirectToAction("AdminLogin");

            MatchesViewModel ViewModel = new MatchesViewModel();
            ViewModel.RoundMatches = new List<SelectListItem>();

            //todo:add date check

            string query = "SELECT match_id,T1.name,T2.name" +
             " FROM Matches, Teams AS T1, Teams AS T2" +
             " WHERE round_number IN (SELECT MAX(round_number) FROM Matches) AND home_team_id = T1.team_id AND away_team_id = T2.team_id";

            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");

            
            for (int i=0;i<result.Count();i++)
            {
                ViewModel.RoundMatches.Add(new SelectListItem { Text = result[i][1].ToString() + 
                                                                        " - " + 
                                                                       result[i][2].ToString(),
                                                                Value = result[i][0].ToString() });
            }

            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult InsertMatchResult(MatchesViewModel VM)
        {
            string query = "UPDATE Matches " +
                            "SET home_team_score = " + VM.UpdatedMatchHomeTeamScore + ", away_team_score = " + VM.UpdatedMatchAwayTeamScore +
                            " WHERE match_id = " + VM.UpdatedMatchID;
            dbreader.ExecuteNonQuery(query);
            ViewBag.Message = " Match Result Updated! ";
            return View(VM);
        }



        //AddView for Admin to add Matches/Players/Teams/NewsPosts
        [HttpGet]
        public IActionResult Add()
        {

            //admin check

            AddViewModel ViewModel = new AddViewModel();
            ViewModel.AllTeams = new List<SelectListItem>();


            string query = " SELECT team_id, name " +
             " FROM Teams";
            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");


            for (int i = 0; i < result.Count(); i++)
            {
                ViewModel.AllTeams.Add(new SelectListItem
                {
                    Text = result[i][1].ToString() ,
                    Value = result[i][0].ToString()
                });
            }
            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult AddMatch(AddViewModel ViewModel)
        {

            string query = "INSERT INTO Matches (home_team_id, away_team_id, round_number, admin_id, date)" +
            " VALUES ('" + ViewModel.Match.HomeTeamId + "','" + ViewModel.Match.AwayTeamId + "','" + ViewModel.Match.RoundNumber + "','" + HttpContext.Session?.GetInt32("AdminID") + "','" + ViewModel.Match.Date + "')";

            dbreader.ExecuteNonQuery(query);
            return View("Add",ViewModel);
        }

        [HttpPost]
        public IActionResult AddPlayer(AddViewModel ViewModel)
        {
           
            string query = " INSERT INTO Players(name, team_id, type, birth_date, status, cost)" +
            " VALUES('" + ViewModel.Player.Name + "','" + ViewModel.SelectedTeamID + "', '" + ViewModel.Player.Type + "', '" + ViewModel.Player.BirthDate + "', ' + Good + ','" + ViewModel.Player.Cost+ "')";

            dbreader.ExecuteNonQuery(query);
            return View("Add", ViewModel);
        }

        [HttpPost]
        public IActionResult AddTeam(AddViewModel ViewModel)
        {
            string query = "INSERT INTO Teams (name)" +
                          " VALUES ('" + ViewModel.Team.Name + "')";
           
            dbreader.ExecuteNonQuery(query);
            return View("Add", ViewModel);
        }

        [HttpPost]
        public IActionResult AddNewsPost(AddViewModel ViewModel)
        {

            string query = "INSERT INTO News(date, admin_id, title, body) " +
                          " VALUES ('" + DateTime.Now + "','" + HttpContext.Session?.GetInt32("AdminID") + "', '" + ViewModel.NewsPost.Title + "', '" + ViewModel.NewsPost.Body + "')";

            dbreader.ExecuteNonQuery(query);
            return View("Add", ViewModel);
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
   

        [HttpPost]
        public IActionResult Signup(SignupViewModel signupViewModel)
        {
            Users user = signupViewModel.User;
            if (signupViewModel.PasswordAgain != user.Password)
                return View();
            if (user.Email == "")
                return View();

            //check model state validation
          //  if(ModelState != null && ModelState.IsValid)





            //Validate
            var model = _DbContext.Users.FirstOrDefault(r => r.Username == user.Username);
            if (model != null)
            {
                return View();
                //  return RedirectToAction("Index", new { id = Model.Id });

            }
            else
            {
                
                _DbContext.Users.Add(user);
                _DbContext.SaveChanges();
                HttpContext.Session.SetInt32("ID",user.UserId);

                return RedirectToAction("Index");
                
            }
        
           // Users User;
           
            //User = signupViewModel.User;
            //do validation
            //check if it already exist in database

        //    _DbContext.Users.Add(User);
        //    _DbContext.SaveChanges();
           
        }
        [HttpGet]
        public IActionResult AdminLogin()
        {
            if (HttpContext.Session?.GetInt32("AdminID") != null)
                return RedirectToAction("Index");
            return View();
        }

        public IActionResult Competitions()
        {if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Index");
            CompetitionsViewModel competitionsViewModel= new CompetitionsViewModel();
      //to do , select all competitons,users participating , points from
      //competitions where user

            return View( );
        }
        [HttpPost]
        public IActionResult Competitions(CompetitionsViewModel competition)
        {
            string query = "select code from Competitions where code =" +
                 competition.newCompetition.Code;
            string name = competition.newCompetition.Name;
            string code = competition.newCompetition.Code;
      List<object[]> result=  (List<object[]>)    dbreader.GetData(query, "List");
             if (result.Count != 0||code==null||name==null)
            {
                ViewBag.Message = "Invalid Competition name or code ! ";
                return View();
            }
            ViewBag.Message = " Competition Created  Successfully ! ";
            query = "insert into Competitions(name,code,admin_id)" +
                "values('" + name + "','" + code + "','" + HttpContext.Session.GetInt32("ID") + "')";
            dbreader.ExecuteNonQuery(query);

            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(Admins admin)
        {
            var Model =
              dbreader.GetData("SELECT admin_id from Admins where username='" + admin.Username + "' AND password='" + admin.Password + "'", "List");


            var LoginAdmin = (List<object[]>)Model;



            if (LoginAdmin != null && LoginAdmin.Count() != 0)
            {
                HttpContext.Session?.SetInt32("AdminID", admin.AdminId);
                return RedirectToAction("Index");
            }
            else
                return View("Login");

        }
    }
}
