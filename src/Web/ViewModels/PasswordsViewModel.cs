using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels
{
    public class PasswordsViewModel
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
