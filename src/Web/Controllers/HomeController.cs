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
using System.Security.Cryptography;
using System.Text;
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
            string query = "SELECT T1.name,home_team_score,away_team_score,T2.name,bet_status " +
                           " FROM Bets_History,Matches,Teams AS T1,Teams AS T2 " +
                           " WHERE user1_id = "+user_id+" AND Bets_History.match_id = Matches.match_id " +
                           " AND T1.team_id = home_team_id " +
                           " AND T2.team_id = away_team_id";


            ViewModel.ResultList = (List<object[]>)dbreader.GetData(query, "List");
            
            
            
            
                query = "SELECT T1.name,home_team_score,away_team_score,T2.name,bet_status " +
                           " FROM Bets_History,Matches,Teams AS T1,Teams AS T2 " +
                           " WHERE user2_id = " + user_id + " AND Bets_History.match_id = Matches.match_id " +
                           " AND T1.team_id = home_team_id " +
                           " AND T2.team_id = away_team_id";
                var Result = (List<object[]>)dbreader.GetData(query, "List");
                if( Result.Count != 0||ViewModel.ResultList.Count!=0)
                    empty = false;
            for (int i = 0; i < Result.Count; i++)
            {
                if ((int)Result[i][4] == 1)
                    Result[i][4] = 0;
               else if ((int)Result[i][4] == 0)
                    Result[i][4] = 1;
            }
            if (empty)
            {
                ViewModel.HaveBetHistory = false;
            }
            else
            {
                ViewModel.HaveBetHistory = true;
            }
            for(int i=0;i<Result.Count;i++)
                ViewModel.ResultList.Add(Result[i]);

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
        [HttpGet]
        public IActionResult ChangePassword()
        {
            PasswordsViewModel VM = new PasswordsViewModel();
            return View(VM);
        }

        [HttpPost]
        public IActionResult ChangePassword(PasswordsViewModel VM)
        {

            string query;

            if (HttpContext.Session?.GetInt32("ID") != null)
            {
                query = " SELECT password " +
                        " FROM Users " +
                        " WHERE user_id = " + HttpContext.Session?.GetInt32("ID");
            }
            else
            {
                query = " SELECT password " +
                        " FROM Admins " +
                        " WHERE admin_id = " + HttpContext.Session?.GetInt32("AdminID");
            }

            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");
            string OldPassword = (string)result[0][0];

            if (OldPassword != VM.oldPassword)
            {
                ViewBag.Message = " Your old password is not correct! ";
                return View();
            }

            if (HttpContext.Session?.GetInt32("ID") != null)
            {
                query = " UPDATE Users " +
                        " SET password = '" + VM.newPassword + "'" +
                        " WHERE user_id = " + HttpContext.Session?.GetInt32("ID");
            }
            else {
                query = " UPDATE Admins " +
                        " SET password = '" + VM.newPassword + "'" +
                        " WHERE admin_id = " + HttpContext.Session?.GetInt32("AdminID");
            }
            dbreader.ExecuteNonQuery(query);
            ViewBag.Message = " Password changed! ";
            return View();
        }

        [HttpPost]
        public IActionResult Login(Users user)
        {
            user.Password = CalculateMD5Hash(user.Password);

            //Validate
            var Model =
                dbreader.GetData("SELECT user_id from Users where username='"+user.Username+ "' AND password='"+user.Password+"'" ,"List");


             var LoginViewModel = (List<object[]>) Model;



            if (LoginViewModel != null && LoginViewModel.Count()!=0)
            {
                HttpContext.Session.SetInt32("ID", (int)LoginViewModel[0][0]); 
            }
            return RedirectToAction("Index");
        }


        //get request Index Method accessed by /home/Index 
        [HttpGet]
        public IActionResult Index()
        {
         
            return View();
        }

        [HttpGet]
        public IActionResult DeletePlayer(int ? id)
        {
            string query = "Delete FROM Players where player_id = " + id;

            dbreader.ExecuteNonQuery(query);

            return RedirectToAction("AdminIndex");
        }

        [HttpGet]
        public IActionResult PlayerLeftLeague(int? id)
        {
            string query = " UPDATE Players " +
                           " SET status = 'Left' " +
                           " WHERE player_id = " + id;

            dbreader.ExecuteNonQuery(query);

            return RedirectToAction("AdminIndex");
        }

        [HttpGet]
        public IActionResult DeleteMatch(int? id)
        {
            string query = "Delete FROM Matches where match_id = " + id;

            dbreader.ExecuteNonQuery(query);

            return RedirectToAction("AdminIndex");
        }

        [HttpPost]
        public IActionResult StartRound()
        {
            string query = " SELECT squad_id,player_id " +
                           " FROM Squads_Players_Temp " +
                           " WHERE status = 1";

            List<object[]> SquadPlayers = (List<object[]>)dbreader.GetData(query, "List");

            for (int i = 0; i < SquadPlayers.Count(); i++)
            {
                query = " INSERT INTO Squads_Players_Lineup(squad_id,player_id,round) " +
                        " VALUES(" + (int)SquadPlayers[i][0] + "," + (int)SquadPlayers[i][1] + ",(SELECT MAX(round_number) FROM Matches))";
                dbreader.ExecuteNonQuery(query);
            }
            return RedirectToAction("Player");
        }

        [HttpGet]
        public IActionResult News()
        {
            string query = " SELECT title,body " +
                           " FROM News ";

            List<object[]> AllNews = (List<object[]>)dbreader.GetData(query, "List");

            return View(AllNews);
        }

        [HttpGet]
        public IActionResult AdminIndex()
        {
            AdminIndexViewModel VM = new AdminIndexViewModel();
            VM.Players = new List<object[]>();
            VM.Matches = new List<object[]>();

            string query = " SELECT player_id, Players.name,Teams.name,type " +
                           " FROM Players,Teams " +
                           " WHERE Players.team_id=Teams.team_id";

            VM.Players = (List<object[]>)dbreader.GetData(query, "List");

            query = " SELECT match_id , T1.name , home_team_score , date , away_team_score , T2.name , round_number" +
                    " FROM Matches, Teams AS T1, Teams AS T2" +
                    " WHERE home_team_id = T1.team_id AND away_team_id = T2.team_id" +
                    " ORDER BY round_number ";

            VM.Matches = (List<object[]>)dbreader.GetData(query, "List");

            return View(VM);
        }
        
        [HttpGet]
        public IActionResult Admin1ControlPanel()
        {

            //admin check

            if (HttpContext.Session?.GetInt32("AdminID") == null)
                return RedirectToAction("AdminLogin");

            AddViewModel ViewModel = new AddViewModel();
            ViewModel.AllTeams = new List<SelectListItem>();
            ViewModel.AllPlayers = new List<SelectListItem>();
            ViewModel.PlayerActions = new List<SelectListItem>();


            string query = " SELECT team_id, name " +
             " FROM Teams";
            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");


            for (int i = 0; i < result.Count(); i++)
            {
                ViewModel.AllTeams.Add(new SelectListItem
                {
                    Text = result[i][1].ToString(),
                    Value = result[i][0].ToString()
                });
            }

            query = " SELECT player_id, name " +
             " FROM Players";
            result = (List<object[]>)dbreader.GetData(query, "List");


            for (int i = 0; i < result.Count(); i++)
            {
                ViewModel.AllPlayers.Add(new SelectListItem
                {
                    Text = result[i][1].ToString(),
                    Value = result[i][0].ToString()
                });
            }

            List<string> PlayerActionsTexts = new List<string>
            { "Scored a goal","Made assist","Scored own goal","Got yellow card","Got red card", "Saved a goal","Received a goal","Saved a penalty","Missed a penalty"};

            List<string> PlayerActionsValues = new List<string>
            { "score_goal","make_assist","score_own_goal","get_yellow_card","get_red_card", "save_goal","recieve_goal","save_penalty","miss_penalty"};

            for (int i = 0; i < PlayerActionsTexts.Count(); i++)
            {
                ViewModel.PlayerActions.Add(new SelectListItem
                {
                    Text = PlayerActionsTexts[i],
                    Value = PlayerActionsValues[i]
                });
            }

            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult EndMatch(AddViewModel ViewModel)
        {
            //fetch the match id from the teams and round number
            string query = "select match_id,home_team_score,away_team_score from Matches where home_team_id= " + ViewModel.Match.HomeTeamId +
                           " AND away_team_id= " + ViewModel.Match.AwayTeamId
                           + " AND round_number= " + ViewModel.Match.RoundNumber;
            ViewModel.Match.MatchId = (int)  ( (List<object[]>) dbreader.GetData(query, "List"))[0][0];
            ViewModel.Match.HomeTeamScore=(int)((List<object[]>)dbreader.GetData(query, "List"))[0][1];
            ViewModel.Match.AwayTeamScore = (int)((List<object[]>)dbreader.GetData(query, "List"))[0][2];

            int winnerId = ViewModel.Match.HomeTeamScore > ViewModel.Match.AwayTeamScore?
                  ViewModel.Match.HomeTeamScore : ViewModel.Match.AwayTeamScore;

            bool tie = ViewModel.Match.HomeTeamScore == ViewModel.Match.AwayTeamScore;



            //fetch all the bets for the match that will be ended now
            query = "select user1_id,user2_id,team1_id,team2_id,points from Bets where match_id="+ ViewModel.Match.MatchId;
            List<object[]> Bets = (List<object[]>)dbreader.GetData(query, "List");

           
            //todo:raga3 l ele mt2blsh el bet bta3o floso
            query = "select user1_id,points from Bets_Requests where match_id=" + ViewModel.Match.MatchId;
            List<object[]> Refund = (List<object[]>)dbreader.GetData(query, "List");
            foreach (var refund in Refund)
            {
                int user1Id = (int)refund[0];
                int points = (int)refund[1];
                query = "update Squads set points=points+ " + points + " where user_id= " + user1Id;
            }

            //delete all bet requests that haven't been accepted yet //todo:this should be done before the game begins
            query = "delete from Bets_Requests where match_id=" + ViewModel.Match.MatchId;
            dbreader.ExecuteNonQuery(query);

            //delete all the bets on the current match
            query = "delete from Bets where match_id=" + ViewModel.Match.MatchId;
            dbreader.ExecuteNonQuery(query);

            //todo:add an indication to matches table to indicate that the match has ended
            
        
            //update winners and losers points
            //move bets to bet history
            //user1_id,user2_id,team1_id,team2_id,points
            //0       ,1        ,2       , 3     ,4
            foreach (var bet in Bets)
            {
                int user1Id = (int) bet[0];
                int user2Id = (int) bet[1];
                int team1Id = (int) bet[2];
                int team2Id = (int) bet[3];
                int points = (int) bet[4];
                string query2 = "";

                if (tie || team2Id == winnerId)
                {
                    query = "Update Squads set points = points + " + points + " where user_id=" + user2Id;
                    query2= "insert into Bets_History values (" + user1Id + ",0," + ViewModel.Match.MatchId + "," + user2Id + "," +points+
            ")";
                    
                }
                else
                {
                    query = "Update Squads set points = points + " + points + " where user_id=" + user1Id;
                    query2 = "insert into Bets_History values (" + user1Id + ",1," + ViewModel.Match.MatchId + "," + user2Id + "," + points + ")";

                }

                dbreader.ExecuteNonQuery(query);
                 dbreader.ExecuteNonQuery(query2);
            }

            return View("Admin1ControlPanel", ViewModel);
        }
        [HttpPost]
        public IActionResult AddMatch(AddViewModel ViewModel)
        {

            string query = "INSERT INTO Matches (home_team_id, away_team_id, round_number, admin_id, date)" +
            " VALUES ('" + ViewModel.Match.HomeTeamId + "','" + ViewModel.Match.AwayTeamId + "','" + ViewModel.Match.RoundNumber + "','" + HttpContext.Session?.GetInt32("AdminID") + "','" + ViewModel.Match.Date + "')";

            dbreader.ExecuteNonQuery(query);
            return View("Admin1ControlPanel",ViewModel);
        }

        [HttpPost]
        public IActionResult UpdateMatchDate(AddViewModel ViewModel)
        {

            string query = " UPDATE Matches " +
                    " SET date = '" + ViewModel.Match.Date + "' " +
                    " WHERE match_id = (SELECT match_id " +
                                      " FROM Matches " +
                                      " WHERE ( home_team_id = " + ViewModel.Match.HomeTeamId + " OR away_team_id = " + ViewModel.Match.AwayTeamId + " ) AND round_number = " + ViewModel.Match.RoundNumber + ")";


            dbreader.ExecuteNonQuery(query);
            return View("Admin1ControlPanel", ViewModel);
        }

        [HttpPost]
        public IActionResult AddPlayer(AddViewModel ViewModel)
        {
           
            string query = " INSERT INTO Players(name, team_id, type, birth_date, status, cost,tshirt_number)" +
            " VALUES('" + ViewModel.Player.Name + "','" + ViewModel.SelectedTeamID + "', '" + ViewModel.Player.Type + "', '" + ViewModel.Player.BirthDate + "', ' Good ','" + ViewModel.Player.Cost+ "','" + ViewModel.Player.TshirtNumber + "')";

            dbreader.ExecuteNonQuery(query);
            return View("Admin1ControlPanel", ViewModel);
        }

        [HttpPost]
        public IActionResult TransferPlayer(AddViewModel ViewModel)
        {

            string query = " UPDATE Players " +
                           " SET team_id = " + ViewModel.SelectedTeamID + " , tshirt_number = " + ViewModel.NewTshirtNumber +
                           " WHERE player_id = ( SELECT player_id " +
                                               " FROM Players " +
                                               " WHERE name = '" + ViewModel.Player.Name + "' AND team_id = " + ViewModel.Player.TeamId + " AND tshirt_number = " + ViewModel.Player.TshirtNumber + ")";
            dbreader.ExecuteNonQuery(query);
            return View("Admin1ControlPanel", ViewModel);
        }

        [HttpPost]
        public IActionResult AddTeam(AddViewModel ViewModel)
        {
            string query = "INSERT INTO Teams (name)" +
                          " VALUES ('" + ViewModel.Team.Name + "')";
           
            dbreader.ExecuteNonQuery(query);
            return View("Admin1ControlPanel", ViewModel);
        }

        [HttpPost]
        public IActionResult AddNewsPost(AddViewModel ViewModel)
        {

            string query = "INSERT INTO News(date, admin_id, title, body) " +
                          " VALUES ('" + DateTime.Now + "','" + HttpContext.Session?.GetInt32("AdminID") + "', '" + ViewModel.NewsPost.Title + "', '" + ViewModel.NewsPost.Body + "')";

            dbreader.ExecuteNonQuery(query);
            return View("Admin1ControlPanel", ViewModel);
        }

        [HttpPost]
        public IActionResult UpdatePlayer(AddViewModel ViewModel)
        {

            //Getting Player Team ID
            string query = " SELECT team_id " +
                           " FROM Players " +
                           " WHERE player_id = " + ViewModel.SelectedPlayerID;

            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");
            int TeamID = (int)result[0][0];

            //Getting the match of this round where the player made an action 
            query = " SELECT match_id " +
                    " FROM Matches " +
                    " WHERE ( home_team_id = " + TeamID + " OR away_team_id = " + TeamID + " ) AND round_number = 1 ";

            result = (List<object[]>)dbreader.GetData(query, "List");
            int MatchID = (int)result[0][0];

            int Goals = 0;

            //Update match score if goals scored
            if (ViewModel.SelectedAction == "score_goal" || ViewModel.SelectedAction == "score_own_goal")
            {
                if (ViewModel.SelectedAction == "score_goal")
                    Goals++;

                query = " UPDATE Matches ";

                //if player in home team increment his team score if he scored a goal
                //or increment other team score if he scored an own goal
                if (ViewModel.SelectedAction == "score_goal")
                    query += " SET home_team_score = home_team_score + 1 ";
                else
                    query += " SET away_team_score = away_team_score + 1 ";

                query += " WHERE match_id = " + MatchID + " AND home_team_id = " + TeamID;
                int RowsAffected = dbreader.ExecuteNonQuery(query);
                //RowsAffected = 0 => the player is in the away team
                if (RowsAffected == 0)
                {
                    query = " UPDATE Matches ";

                    if (ViewModel.SelectedAction == "score_goal")
                        query += " SET away_team_score = away_team_score + 1 ";
                    else
                        query += " SET home_team_score = home_team_score + 1 ";

                    query += " WHERE match_id = " + MatchID + " AND away_team_id = " + TeamID;
                    dbreader.ExecuteNonQuery(query);
                }
            }

            //Update player points in the match
            query = " UPDATE Players_Matches_Played " +
                    " SET points = points + (SELECT " + ViewModel.SelectedAction +
                                           " FROM Points_Manager " +
                                           " WHERE player_type = ( SELECT type " +
                                                                 " FROM Players " +
                                                                 " WHERE player_id = " + ViewModel.SelectedPlayerID + " )) , goals = goals + " + Goals +
                   " WHERE match_id = " + MatchID + " AND player_id = " + ViewModel.SelectedPlayerID;

            dbreader.ExecuteNonQuery(query);

            //Update points of the squads having this player
            query = " UPDATE Squads " +
                    " SET points = points + (SELECT points " +
                                           " FROM Players_Matches_Played " +
                                           " WHERE player_id  = " + ViewModel.SelectedPlayerID + " AND match_id = " + MatchID + " )" +
                    " WHERE squad_id = 1";

            dbreader.ExecuteNonQuery(query);


            return View("Admin1ControlPanel", ViewModel);
        }

        [HttpGet]
        public IActionResult Signup()
        {
            if (HttpContext.Session?.GetInt32("ID") != null)
                return RedirectToAction("Index");
            return View();
        }

        /* [HttpGet]
            public IActionResult UpdateHash()
         {
             string query = "select user_id,password from Users";
                  List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");
             foreach (var user in result)
             {
                 query="update Users set password='"+CalculateMD5Hash((string)user[1])+"' where user_id="+
                 (int)user[0];
                 dbreader.ExecuteNonQuery(query);
             }
             query = "select admin_id,password from Admins";
             result = (List<object[]>)dbreader.GetData(query, "List");
             foreach (var user in result)
             {
                 query = "update Admins set password='" + CalculateMD5Hash((string)user[1]) + "' where admin_id=" +
                 (int)user[0];
                 dbreader.ExecuteNonQuery(query);
             }
             return RedirectToAction("Index");
         }*/

        [HttpPost]
        public IActionResult Signup(SignupViewModel signupViewModel)
        {
            
            Users user = signupViewModel.User;
            if (signupViewModel.PasswordAgain != user.Password)
                return View();
            if (user.Email == "")
                return View();
            user.Password = CalculateMD5Hash(user.Password);
            //check model state validation
            //  if(ModelState != null && ModelState.IsValid)



            //Validate
            string query = "select * from Users where username=" + signupViewModel.User.Username;
            dbreader.GetData(query, "List");
            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");
            if(result.Count!=0)
                return RedirectToAction("Signup");

            query = "insert into Users values ('" + signupViewModel.User.Username + "','" + signupViewModel.User.Password + "','" +
                    signupViewModel.User.FirstName + "','" + signupViewModel.User.LastName + "','" + signupViewModel.User.Email + "')";
            dbreader.ExecuteNonQuery(query);
                

                HttpContext.Session.SetInt32("ID",user.UserId);
                return RedirectToAction("Index");

       
           
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
            List<object[]> result = (List<object[]>)dbreader.GetData(query, "List");
            return result;
        }

        [HttpPost]
        public IActionResult AdminLogin(Admins admin)
        {
            admin.Password = CalculateMD5Hash(admin.Password);
            var Model =
              dbreader.GetData("SELECT admin_id from Admins where username='" + admin.Username + "' AND password='" + admin.Password + "'", "List");


            var LoginAdmin = (List<object[]>)Model;



            if (LoginAdmin != null && LoginAdmin.Count() != 0)
            {
                HttpContext.Session?.SetInt32("AdminID", (int)LoginAdmin[0][0]);
                return RedirectToAction("AdminIndex");
            }
            else
            {
                ViewBag.Message = " Wrong Username or Password! ";
                return View("AdminLogin");
            }

        }
        public string CalculateMD5Hash(string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("x2"));

            }

            return sb.ToString();

        }


        public IActionResult Error()
        {
            return View();
        }
    }

   
  
}
