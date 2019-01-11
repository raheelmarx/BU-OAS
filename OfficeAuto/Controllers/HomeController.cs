using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeAuto.Models;

namespace OfficeAuto.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DetailedStats() {
            return View();
        }
        public IActionResult Inbox()
        {
            return View();
        }
        public IActionResult Outbox()
        {
            return View();
        }
        public IActionResult Drafts()
        {
            return View();
        }
        public IActionResult DFAsent()
        {
            return View();
        }
        public IActionResult DFAReceived()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
