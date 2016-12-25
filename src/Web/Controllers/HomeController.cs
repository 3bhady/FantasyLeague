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

        /*[HttpGet]
        public IActionResult Matches()
        {

            string query = "SELECT T1.name,home_team_score,away_team_score,T2.name" +
             " FROM Matches, Teams AS T1, Teams AS T2" +
             " WHERE round_number IN (SELECT MAX(round_number) FROM Matches) AND home_team_id = T1.team_id AND away_team_id = T2.team_id";

            var Model = dbreader.GetData(query, "List");
            List<object[]> matches = (List < object[] >) Model;

            return View(matches);
        }
        */
        [HttpGet]
        public IActionResult AddMatches()
        {
            //if (HttpContext.Session?.GetInt32("AdminID") == null)
              //  HttpContext.Session?.SetInt32("AdminID",1);
            return View();
        }

        [HttpPost]
        public IActionResult AddMatches(Matches match)
        {

            string query = "INSERT INTO Matches (home_team_id, away_team_id, round_number, admin_id, date)" +
            " VALUES ('"+match.HomeTeamId+ "','" + match.AwayTeamId + "','" + match.RoundNumber + "','" + HttpContext.Session?.GetInt32("AdminID") + "','" + match.Date + "')";

            dbreader.ExecuteNonQuery(query);

            return RedirectToAction("AddMatches",match);
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
        //get requests of logged in user as well as his competitions 
        [HttpGet]
        public IActionResult Competitions()
        {if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Index");
            int id = (int)HttpContext.Session.GetInt32("ID");
            CompetitionsViewModel competitionsViewModel= new CompetitionsViewModel();
            //getting all my competitions
            competitionsViewModel.userCompetitions = GetMyCompetitions();

            //getting all requests for my competitions !!
            string query = "select c.name,c.code,c.competition_id,u.username,u.user_id from Competitions as c ,"+
" User_Competitions_Requests as r ,Users as u"+
" where u.user_id = r.user_id and r.competition_id = c.competition_id and "+
"c.admin_id = "+id;
            ViewBag.Message = TempData["Message"];
         List<Object[]> competitionsRequests =(List<Object[]>)dbreader.GetData(query,"List");
            competitionsViewModel.competitionsRequests = competitionsRequests;
            return View(competitionsViewModel);
        }
        [HttpGet]
        public IActionResult AcceptCompetitionRequest(int id,int key)
        { int userId = id;
            int compId = key;
            string query = "insert into User_Competitions_Members Values( " +
               userId + " , " + compId +" )";

        int result=    dbreader.ExecuteNonQuery(query);
            query = "delete from User_Competitions_Requests where user_id = " +
                userId + " and competition_id = " + compId;
            result = dbreader.ExecuteNonQuery(query);
            return RedirectToAction("Competitions");
        }
        //requested when creating a competition 
        [HttpPost]
        public IActionResult CreateCompetition(CompetitionsViewModel competition)
        {
            string query = "select code from Competitions where code = '" +
                competition.newCompetition.Code +"'" ;
            string name = competition.newCompetition.Name;
            string code = competition.newCompetition.Code;
      List<object[]> result=  (List<object[]>)    dbreader.GetData(query, "List");
             if (result.Count != 0||code==null||name==null)
            {
                TempData["Message"] = "Invalid Competition name or code ! ";
                return RedirectToAction("Competitions");
            }
            TempData["Message"] = " Competition Created  Successfully ! ";
            query = "insert into Competitions(name,code,admin_id)" +
                "values('" + name + "','" + code + "','" + HttpContext.Session.GetInt32("ID") + "')";
            dbreader.ExecuteNonQuery(query);
            //inserting the admin
             query = "select competition_id from Competitions where code = '" + competition.newCompetition.Code + "'";
            List<object[]> cmp = (List<object[]>)dbreader.GetData(query, "List");
            query = " insert into User_Competitions_Members values (" + HttpContext.Session.GetInt32("ID") + "," +(int)cmp[0][0] + ")";
            int x = dbreader.ExecuteNonQuery(query);

            return RedirectToAction("Competitions");
        }
        //requested when joining a comp etition .
        public IActionResult JoinCompetition(CompetitionsViewModel competition)
        {
            string query = "select competition_id,admin_id from Competitions where code = '" + competition.newCompetition.Code+"'";
            List<object[]> cmp = (List<object[]>)dbreader.GetData(query, "List");
            if (cmp.Count==0)
            {
                TempData["Message"] = " dude, there is no competition with such a code ! ";
                return RedirectToAction("Competitions");
            }
            if ((int)cmp[0][1] == (int) HttpContext.Session.GetInt32("ID"))
            {
                TempData["Message"] = " What if i told you that you are the admin of that competition "+
                    " does the admin need request ?!! ";
                return RedirectToAction("Competitions");
            }
            //check if he is a member 
            query = " select u.user_id from User_Competitions_Members as u  " +
                " where   u.user_id= " + (int)HttpContext.Session.GetInt32("ID") + " and " +
                " competition_id = " + cmp[0][0];
            List<object[]> y =(List<object[]>) dbreader.GetData(query,"List");
            if ( y.Count !=0)
            {
                TempData["Message"] = " remember, you are a member :)  ";
                return RedirectToAction("Competitions");
            }
            query = " insert into User_Competitions_Requests values (" + HttpContext.Session.GetInt32("ID") + "," + cmp[0][0] + ")";
            int x = dbreader.ExecuteNonQuery(query);
            
            if (x == -1) {
                TempData["Message"] = " What if i told you that you have already sent request to join the competition !! ";
            }
          
            return RedirectToAction("Competitions");
        }


        //view certain competition by id
        [HttpGet]
        public IActionResult ViewCompetition(int id)
        {
            string query = "select u.username,s.points from users as u ,  Squads as s" +
            " where u.user_id=s.user_id and  u.user_id in (select user_id from User_Competitions_Members where competition_id = " + id + " )  order by u.points desc";
            List<object[]> competitionInfo=( List < object[] > )dbreader.GetData(query,"List");
            return View(competitionInfo);
        }
        public List<object[]> GetMyCompetitions()
        {
            string query = "select competition_id,name,code,admin_id from Competitions where " +
                 " competition_id in " + " ( select competition_id from " +
                 " User_Competitions_Members  where user_id = " + HttpContext.Session.GetInt32("ID") + " )";
            List<object[]> result = (List < object[] > )dbreader.GetData(query, "List");
            return result;
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
