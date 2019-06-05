using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Areas.System.Models
{
    public class UserIndexViewModel
    {
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public string StatusMessage { get; set; }
    }
}
