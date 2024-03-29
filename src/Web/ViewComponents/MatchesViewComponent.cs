﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DBEntities;
using Web.Models;

namespace Web.ViewComponents
{
    public class MatchesViewComponent : ViewComponent
    {
        private FantasyLeagueContext _DbContext;
        private IDBReader dbreader;

        public MatchesViewComponent(FantasyLeagueContext Db, IDBReader dr)
        {
            dbreader = dr;
            _DbContext = Db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string query = "SELECT T1.name,home_team_score,date,away_team_score,T2.name, match_id" +
             " FROM Matches, Teams AS T1, Teams AS T2" +
             " WHERE round_number IN (SELECT MAX(round_number) FROM Matches) AND home_team_id = T1.team_id AND away_team_id = T2.team_id";

            List<object[]> matches = (List<object[]>)dbreader.GetData(query, "List");

            await _DbContext.SaveChangesAsync();

            return View(matches);
        }
    }
}
