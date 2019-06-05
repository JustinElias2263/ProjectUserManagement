using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Areas.HR.Controllers
{
    [Area("HR")]
    [Authorize(Roles = "Administrator, System, HR")]
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var requests = await _context.NameChangeRequests.ToListAsync();
            return View(requests);
        }
    }
}