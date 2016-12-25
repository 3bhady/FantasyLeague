using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.DBEntities;

namespace Web.ViewComponents
{
    public class LoginViewComponent : ViewComponent
    {
        private FantasyLeagueContext _DbContext;
        private IDBReader dbreader;

        public LoginViewComponent(FantasyLeagueContext Db, IDBReader dr)
        {
            dbreader = dr;
            _DbContext = Db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            await _DbContext.SaveChangesAsync();

            Users user = new Users();

            return View(user);
        }
    }
}
