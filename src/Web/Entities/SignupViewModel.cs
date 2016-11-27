using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Entities
{
    public class SignupViewModel
    {
        public Users User { get; }

        public SignupViewModel()
        {
            User = new Users();
        }

        [Required]
        public string PasswordAgain { get; set; }



    }
}
