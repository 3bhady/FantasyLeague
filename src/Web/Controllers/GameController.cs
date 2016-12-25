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


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers
{
    public class GameController : Controller
    {
        // define private object holding the database context 
        private FantasyLeagueContext _DbContext;
        private IDBReader dbreader;
        public GameController(FantasyLeagueContext Db, IDBReader dr)
        {

            dbreader = dr;
            _DbContext = Db;
        }
        public IActionResult Profile()
        {
            if ((int)HttpContext.Session.GetInt32("ID") == null)
            {
                return (new NotFoundResult());
            }
            return View();
        }
        [HttpGet("squad")]
        public IActionResult Squad()
        {
            if ((int)HttpContext.Session.GetInt32("ID") == null)
            {
                return (new NotFoundResult());
            }
            ViewBag.message = TempData["message"];
            SquadViewModel squadViewModel = new SquadViewModel();
            string query = "select sq.squad_id,sq.user_id,sq.defenders, " +
            "sq.midfielders,sq.strikers,t.name,p.name,u.points " +
            "from Squads as sq,Teams as t,Players as p , Users as u " +
            "where sq.user_id = u.user_id and u.user_id =" +
            HttpContext.Session.GetInt32("ID");
            List<object[]> squad =(List<object[]>) dbreader.GetData(query, "List");
            squadViewModel.squad = squad;
            return View(squadViewModel);
        }
        public IActionResult HelloAjax()
        {
            return Content("Hellosadasdsa Ajax", "text/plain");
        }
        [HttpGet("players/{key?}")]
        public IActionResult GetPlayers(string key)
        {
            string query;
            if (key == "defender" || key == "striker" || key == "goalKeeper" || key == "midfielder")
            {
                query = "select P.player_id,P.name as pname,P.team_id,P.position,P.image as pimage,P.status,P.cost,T.team_id as tid,T.image as timage,T.name as tname from Players as P,Teams  as T  where P.position ='"+key+ "' and  T.team_id = P.team_id order by P.position";
            }
            else if(key=="all")
            {
             query = "select P.player_id,P.name as pname,P.team_id,P.position,P.image as pimage,P.status,P.cost,T.team_id as tid,T.image as timage,T.name as tname from Players as P,Teams  as T  where T.team_id = P.team_id";

            }
            else
            {
                query = "select P.player_id,P.name as pname,P.team_id,P.position,P.image as pimage,P.status,P.cost,T.team_id as tid,T.image as timage,T.name as tname from Players as P,Teams  as T  where P.team_id= "+key+"and  T.team_id = P.team_id";
            }

                var players = dbreader.GetData(query, "Lst");

         return    Json(players);
        }

        [HttpGet("teams/{key?}")]
        public IActionResult GetTeams(int key)
        {
            string query;
            if (key!=0)
            {
                query = "select * from Teams where team_id = " + key;

            }
            else
            {
                query = "select * from Teams ";
            }

            var teams = dbreader.GetData(query, "Lst");
            return Json(teams);
        }
        //submitting squad building 
        [HttpPost]
        public IActionResult SubmitSquad(SquadViewModel sq)
        {
            if ((int)HttpContext.Session.GetInt32("ID") == null)
            {
                return (new NotFoundResult());
            }
            if (HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToRoute("/home/index");
            }
                if (sq.userSquad.Formation.Length < 1||sq.userSquad.HeroPlayerId == 0 || sq.userSquad.HeroTeamId == 0)

                {

                TempData["message"] = " invalid ";
                return RedirectToAction("Squad");
            }
           
            string[] formation = sq.userSquad.Formation.Split('-');
            int defenders = Int32.Parse(formation[0]);
            int midFielders = Int32.Parse(formation[1]);
            int strikers = Int32.Parse(formation[2]);
            string query = "insert into Squads(user_id,hero_team_id,hero_player_id,defenders,midfielders,strikers) Values ( " +
                HttpContext.Session.GetInt32("ID") + " ," + sq.userSquad.HeroTeamId + " ," +
                sq.userSquad.HeroPlayerId + ", " + defenders + ", " + midFielders + " ," +
                strikers+" )";
            int result= dbreader.ExecuteNonQuery(query);
            
            return RedirectToAction("Squad");
        }
        [HttpGet("myPlayers")]
        public IActionResult GetMyPlayers()
        {
            if ((int)HttpContext.Session.GetInt32("ID") == null)
            {
                return (new NotFoundResult());
            }

            string query = "select p.player_id,p.name,p.team_id,p.position,p.image,p.birth_date,p.status,p.cost,"+
                " t.isPlaying from Players as p,Squads_Players_Temp as t " +
            " where p.player_id in(select player_id from Squads_Players_Temp where squad_id " +
            "  = (select squad_id from Squads where user_id = "+ (int)HttpContext.Session.GetInt32("ID") + " and p.player_id = t.player_id )) ";
            var result = dbreader.GetData(query, "Dictionary");

            return Json(result); 
        }
        //updating user tempFormation after editting the squad
        [HttpPost]
        public IActionResult UpdateTempFormation([FromBody]SwapPlayers sp)
        {
            if ((int)HttpContext.Session.GetInt32("ID") == null)
            {
                return (new NotFoundResult());
            }
            /*
             *
             * int primary = Int32.Parse(  sp.primary);
             int secondary = Int32.Parse(sp.secondary);
             string query1 = "select player_id from Players where id= " + primary;
             string query2 = "delete from Squads_Players_Temp where player_id = " + secondary;
             string query3 = "select squad_id from Squads where user_id = " + (int)HttpContext.Session.GetInt32("ID");
             int squadID = (int)(((List<object[]>)(dbreader.GetData(query3, "List")))[0][0]);
             int result2 = dbreader.ExecuteNonQuery(query2);
             int userID = (int)(((List<object[]>)(dbreader.GetData(query1, "List")))[0][0]);
              // string query4=  "insert into Squads_Players_Temp Values ( '" ++"' , 
              this is for transfer not here ..
              */
            int primary = Int32.Parse(sp.primary);
            int secondary = Int32.Parse(sp.secondary);
            string query3 = "select squad_id from Squads where user_id = " + (int)HttpContext.Session.GetInt32("ID");
            int squadID = (int)(((List<object[]>)(dbreader.GetData(query3, "List")))[0][0]);

            string query1 = "Update Squads_Players_Temp Set isPlaying = 1  where "+
                " player_id = "+primary+" and squad_id = " + squadID;
            string query2 = "Update Squads_Players_Temp Set isPlaying = 0  where " +
               " player_id = " + secondary  + " and squad_id = " + squadID;
            int result1 = dbreader.ExecuteNonQuery(query1);
            int result2 = dbreader.ExecuteNonQuery(query2);






            return Json(sp);
        }
        public IActionResult Transfer()
        {
            if ((int)HttpContext.Session.GetInt32("ID") == null)
            {
                return (new NotFoundResult());
            }
            return View();
        }
        [HttpGet("UpdateUserInfo")]
        public IActionResult UpdateUserInfo()
        {
            if ((int)HttpContext.Session.GetInt32("ID") == null)
            {
                return (new NotFoundResult());
            }
            string query="select Users.user_id,username,Squads.points ,Squads.money  from Users,Squads "+
                " where Users.user_id=Squads.user_id and Users.user_id = "+ HttpContext.Session.GetInt32("ID");

            var result = dbreader.GetData(query, "Dictionary");
            return Json(result);
        }
        [HttpPost]
        public IActionResult UpdateFormation([FromBody] string[] result)
        {
            if((int)HttpContext.Session.GetInt32("ID")==null)
            {
                return (new NotFoundResult());
            }
            List<int> arr = new List<int>();
            for (int i=0; i<result.Length; i++)
            {
                arr.Add(Int32.Parse(result[i]));

            }
            for(int i=0; i<arr.Count-1; i+=2)
            { int k = arr[i];
                int j = arr[i+1];
                string query = "Update Squads_Players_Temp Set player_id = " + j + " where player_id = " + k ;
           int queryResult = dbreader.ExecuteNonQuery(query);
            }
            string query2 = "Update Squads Set money = " + arr[arr.Count -1] + " where user_id =  " + (int)HttpContext.Session.GetInt32("ID");
            int query2Result = dbreader.ExecuteNonQuery(query2);


            return Json(result);
        }
        public string test()
        {
            return "sadas";
        }
    }
}
