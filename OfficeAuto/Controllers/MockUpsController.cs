using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OfficeAuto.Data;
using OfficeAuto.Models.DB;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace OfficeAuto.Controllers
{
    public class MockUpsController : Controller
    {
        private readonly OfficeAutoDBContext _context;

        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;

        public MockUpsController(OfficeAutoDBContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateCase()
        {
            //var userid = _userManager.GetUserId(HttpContext.User);
            //var username = _userManager.GetUserName(HttpContext.User);

            //var @users = await _userManager.Users.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Email }).ToListAsync();// Get Users On base of loggedIn User
            //ViewBag.Users = @users;
            //var refdocs = _context.ReferenceDoc.Where(x => x.CaseId == 10089).ToList();
            //ViewBag.RefDocs = refdocs;
            return View();
        }
        public IActionResult CreateMinute()
        {
            return View();
        }
    }
}