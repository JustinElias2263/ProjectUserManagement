using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.Areas.System.Controllers
{
    [Area("System")]
    [Authorize(Roles = "Administrator, System")]
    public class NotificationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}