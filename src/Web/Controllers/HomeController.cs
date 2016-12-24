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

       
        public IActionResult GetBetById(string id)
        {
            if (HttpContext.Session?.GetInt32("ID") == null)
                return RedirectToAction("Index");
            string query = "select * from Bets_Requests where match_id = "+id;
           var result = dbreader.GetData(query, "dictionary");
            return Json(result);
        }

        [HttpGet]
        public IActionResult ValidateBet(string id)
        {
            MatchesViewModel ViewModel = new MatchesViewModel();
            ViewModel.RoundMatches = new List<SelectListItem>();
            String[] substrings = id.Split('-');
            int user2Id = (int)HttpContext.Session.GetInt32("ID");
            int user1Id = Convert.ToInt32(substrings[0]);
            int matchId = Convert.ToInt32(substrings[1]);
            int betPoints = Convert.ToInt32(substrings[2]);
            int team1Id= Convert.ToInt32(substrings[3]);
            string query = "select points from Squads where user_id='" + user2Id + "'";

            int userPoints = (int) ((List<object[]>) dbreader.GetData(query, "List"))[0][0];
            if (userPoints < betPoints)
            {
                ViewModel.ValidBetAccept = false;

                return View(ViewModel);
            }
            query = "select * from Matches where match_id =" + matchId;
            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");
            int home=(int) result[0][1]; 
            int away=(int) result[0][2];
            int team2Id = home == team1Id ? away : home;
            //user1 , user2,points,matchid,team1,team2
            query = "insert into Bets Values ("+user1Id+","+user2Id+","+betPoints+","+matchId+","+team1Id+","+team2Id+")";
            dbreader.ExecuteNonQuery(query);

            query = "Delete From Bets_Requests where user1_id=" + user1Id + " AND match_id=" +matchId;
            dbreader.ExecuteNonQuery(query);
            return View(ViewModel);
        }

    
        [HttpGet]
        public IActionResult Bets()
        {
            if (HttpContext.Session?.GetInt32("ID") == null)
                return RedirectToAction("Login");
            MatchesViewModel ViewModel = new MatchesViewModel();
            ViewModel.RoundMatches = new List<SelectListItem>();

            //todo:add date check

            string query = "SELECT match_id,T1.name,T2.name" +
                           " FROM Matches, Teams AS T1, Teams AS T2" +
                           " WHERE round_number IN (SELECT MAX(round_number) FROM Matches) AND home_team_id = T1.team_id AND away_team_id = T2.team_id " +
                           " AND match_id not in (select match_id from Bets_Requests where user1_id=" +
                           HttpContext.Session?.GetInt32("ID") + ") " + " AND " +
                           "match_id not in (select match_id from Bets where user1_id=" +
                           HttpContext.Session?.GetInt32("ID") + ") " + " AND " +
                           "match_id not in (select match_id from Bets where user2_id=" +
                           HttpContext.Session?.GetInt32("ID") + ")";
            

            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");
            if (result.Count == 0)
            {
                ViewModel.CanBet = false;
            }

            for (int i = 0; i < result.Count(); i++)
            {
                ViewModel.RoundMatches.Add(new SelectListItem
                {
                    Text = result[i][1] + " - " + result[i][2],
                    Value = result[i][0].ToString()
                });
            }
            query = "select points from Squads where user_id='" + HttpContext.Session?.GetInt32("ID") + "'";

            ViewModel.UserPoints = (int)((List<object[]>)dbreader.GetData(query, "List"))[0][0];
           // ViewModel.ValidBetAccept = false;
            return View(ViewModel);
        }

        public IActionResult MyBets()
        {
           

            bool home = false; //if the player betted on the home team then it's true else it stays false
            bool empty = true; //if there are no available bets then this stays true
            if (HttpContext.Session?.GetInt32("ID") == null)
                return RedirectToAction("Login");
            int user_id = (int) HttpContext.Session?.GetInt32("ID");
            MatchesViewModel ViewModel = new MatchesViewModel();
           // ViewModel.RoundMatches = new List<SelectListItem>();
           // ViewModel.ResultList = new List<object[]>();
            string query = "SELECT T1.name,home_team_score,away_team_score,T2.name " +
                           " FROM Bets_History,Matches, Teams AS T1, Teams AS T2 " +
                           " WHERE user1_id=" + user_id +
                           " AND Bets_History.match_id = Matches.match_id " +
                           " AND (T1.team_id=home_team_id OR T1.team_id=away_team_id) " +
                           " AND (T2.team_id=home_team_id OR T2.team_id=away_team_id)";
            ViewModel.ResultList = (List<object[]>)dbreader.GetData(query, "List");
            if (ViewModel.ResultList.Count != 0)
            {
                home = true;
                empty = false;
            }
            else
            {
                query =    "SELECT T1.name,home_team_score,away_team_score,T2.name" +
                           " FROM Bets_History,Matches, Teams AS T1, Teams AS T2 " +
                           " WHERE user2_id=" + user_id +
                           " AND Bets_History.match_id = Matches.match_id " +
                           " AND (T1.team_id=home_team_id OR T1.team_id=away_team_id) " +
                           " AND (T2.team_id=home_team_id OR T2.team_id=away_team_id)";
                ViewModel.ResultList = (List<object[]>)dbreader.GetData(query, "List");
                if( ViewModel.ResultList.Count != 0)
                    empty = false;
            }
            if (empty)
            {
                ViewModel.HaveBetHistory = false;
            }
            else
            {
                ViewModel.HaveBetHistory = true;
            }

            return View(ViewModel);
        }


        [HttpGet]
        public IActionResult CreateBet()
        {
            if (HttpContext.Session?.GetInt32("ID") == null)
                return RedirectToAction("Login");
            MatchesViewModel ViewModel = new MatchesViewModel();
            ViewModel.RoundMatches = new List<SelectListItem>();

            //todo:add date check

            string query = "SELECT match_id,T1.name,T2.name" +
                           " FROM Matches, Teams AS T1, Teams AS T2" +
                           " WHERE round_number IN (SELECT MAX(round_number) FROM Matches) AND home_team_id = T1.team_id AND away_team_id = T2.team_id " +
                           " AND match_id not in (select match_id from Bets_Requests where user1_id=" +
                           HttpContext.Session?.GetInt32("ID")+") " + " AND " +
                           "match_id not in (select match_id from Bets where user1_id=" +
                           HttpContext.Session?.GetInt32("ID") + ") " + " AND " +
                           "match_id not in (select match_id from Bets where user2_id=" +
                           HttpContext.Session?.GetInt32("ID") + ")";
                ;

            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");

          
            for (int i = 0; i < result.Count(); i++)
            {
                ViewModel.RoundMatches.Add(new SelectListItem
                {
                    Text = result[i][1].ToString() +" - " +result[i][2].ToString(),
                    Value = result[i][0].ToString()
                });
            }
             query = "select points from Squads where user_id='" + HttpContext.Session?.GetInt32("ID") + "'";

            ViewModel.UserPoints = (int)((List<object[]>)dbreader.GetData(query, "List"))[0][0];
            return View(ViewModel);
        }


        [HttpPost]
        public IActionResult CreateBet(MatchesViewModel ViewModel)
        {
            int matchid = ViewModel.UpdatedMatchID;
            int userid = (int)HttpContext.Session?.GetInt32("ID");
            int home = ViewModel.HomeID;
            int homeID;
            int awayID;
            int points = ViewModel.BettedValue;
            string query = "SELECT home_team_id,away_team_id From Matches WHERE match_id='" + matchid+"'";

            //get the home teamid and away team id
            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");
            homeID = (int)result[0][0];
            awayID = (int) result[0][1];

            //check if the points are actually valid
            query = "select points from Squads where user_id='" + userid + "'";

            if (points > (int) ((List<object[]>) dbreader.GetData(query, "List"))[0][0])
                return RedirectToAction("CreateBet");
            
            query = "insert into Bets_Requests values ('"+userid+"','"+matchid+"','"+(home==1?homeID:awayID)+"','"+points+
            "')";
            dbreader.ExecuteNonQuery(query);
            query = "update Squads Set points = points - " + points + " where user_id=" + userid;
            dbreader.ExecuteNonQuery(query);




            return RedirectToAction("CreateBet");
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
        //get requests of logged in user as well as his competitions 
        [HttpGet]
        public IActionResult Competitions()
        {
            if (HttpContext.Session.GetInt32("ID") == null)
                return RedirectToAction("Login");
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
            string query = "select u.username,u.points from users as u " +
            " where u.user_id in (select user_id from User_Competitions_Members where competition_id = " + id + " )  order by u.points desc";
            List<object[]> competitionInfo=( List < object[] > )dbreader.GetData(query,"List");
            return View(competitionInfo);
        }
        public List<object[]> GetMyCompetitions()
        {
            string query = "select competition_id,name,code,admin_id from Competitions where " +
                 " competition_id in " + " ( select competition_id from " +
                 " User_Competitions_Members  where user_id = " + HttpContext.Session.GetInt32("ID") + " )";
            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");
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
