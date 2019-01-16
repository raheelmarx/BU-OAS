using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeAuto.Data;
using OfficeAuto.Models.DB;
using OfficeAuto.Models.ViewModels;

namespace OfficeAuto.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        private readonly OfficeAutoDBContext _context;

        public UsersController(OfficeAutoDBContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            //_userManager.Users
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(string id)
        {
            //  if (id == null)
            //  {
            //      return NotFound();
            //  }
            //  var user = await _userManager.FindByIdAsync(id);
            //  if (user == null)
            //  {
            //      return NotFound();
            //  }
            //  RegisterViewModel regModel = new RegisterViewModel { Id = user.Id, Email= user.Email};
            //  regModel.Title = user.Title;
            //  regModel.FirstName = user.FirstName;
            //  regModel.LastName = user.LastName;
            //  regModel.Address = user.Address;
            //  regModel.CampusId = user.CampusId;
            //  regModel.DeptId = user.DeptId;
            //  regModel.City = user.City;
            //  regModel.Province = user.Province;
            //  regModel.Country = user.Country;
            //  regModel.IsActive = user.IsActive;
            //  ViewData["Campuses"] = new SelectList(_context.Campuses, "Id", "Name");
            //  ViewData["Departments"] = new SelectList(_context.Departments, "Id", "Name");
            //  ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            //  var Statuslist = new SelectList(new[]
            //{
            //      new { ID = "0", Name = "In-Active" },
            //      new { ID = "1", Name = "Active" },
            //      new { ID = "2", Name = "Block" },
            //  },
            //"ID", "Name", 1);

            //  ViewData["Statuslist"] = Statuslist;
            //regModel
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RegisterViewModel regModel)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByIdAsync(regModel.Id);
              
                //user.Title = regModel.Title;
                //user.FirstName = regModel.FirstName;
                //user.LastName = regModel.LastName;
                //user.Email = regModel.Email;
                //user.Address = regModel.Address;
                //regModel.CampusId = regModel.CampusId;
                //user.DeptId = regModel.DeptId;
                //user.City = regModel.City;
                //user.Province = regModel.Province;
                //user.Country = regModel.Country;
                //user.IsActive = regModel.IsActive;

                //await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            //var list = new SelectList(new[]
            //{
            //    new { ID = "0", Name = "In-Active" },
            //    new { ID = "1", Name = "Active" },
            //    new { ID = "2", Name = "Block" },
            //},
            //"ID", "Name", 1);

            //ViewData["list"] = list;
            //ViewData["Campuses"] = new SelectList(_context.Campuses, "Id", "Name");
            //ViewData["Departments"] = new SelectList(_context.Departments, "Id", "Name");
            //ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }


        public IActionResult AssignAOLAOD(string id) {
            // AssignAOLAODViewModel model = new AssignAOLAODViewModel();
            //  model.UserId = id;
            //  ViewData["Users"] = new SelectList(_userManager.Users, "FirstName", "FirstName");
           // model
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignAOLAOD(AssignAOLAODViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var users = _context.UserAolAod.Where(x => x.UserId == model.UserId && x.Status == 1).FirstOrDefault();
                //UserAolAod user = new UserAolAod();
                //user.Type = model.Type;
                //user.UserId = model.UserId;
                //user.AoUserid = model.AOUserId;
                //user.DateAssigned = model.DateAssigned;
                //user.DateExpired = model.DateExpired;
                //user.DateAdded = DateTime.Now;
                //_context.UserAolAod.Add(user);
                // _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
           // ViewData["Users"] = new SelectList(_userManager.Users, "FirstName", "FirstName");
            return View();
        }
    }
}