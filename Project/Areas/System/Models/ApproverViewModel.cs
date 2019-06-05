using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Areas.System.Models
{
    public class ApproverViewModel
    {
        public IEnumerable<ApplicationUser> Approver { get; set; }
        public IEnumerable<ApplicationUser> UserList { get; set; }
    }
}
