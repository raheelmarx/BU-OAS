using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using OfficeAuto.Models.DB;
using OfficeAuto.Data;
using OfficeAuto.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using OfficeAuto.Models.ViewModels;

namespace OfficeAuto.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private RoleManager<ApplicationRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly OfficeAutoDBContext _context;

        public RolesController(OfficeAutoDBContext context,UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
       

        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }
        public IActionResult Create()
        {
            ViewData["Departments"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole();
                role.Name=roleViewModel.Name;
                role.DeptId = roleViewModel.DeptId;
                IdentityResult roleresult;
                roleresult = await _roleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    //ModelState.AddModelError("", roleresult.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            roleModel.DeptId = role.DeptId;
            ViewData["Departments"] = new SelectList(_context.Departments, "Id", "Name");
            return View(roleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;
                role.DeptId = roleModel.DeptId;
                await _roleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            ViewData["Departments"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
                if (id == null)
                {
                    return NotFound();
                }
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
                var users=_userManager.GetUsersInRoleAsync(role.Name).Result;

                IdentityResult result;

                if (users.Count>0)
                {
                TempData["errorMsg"] = "User exist with this role , please delete user first.";
                }
                else
                {
                result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    TempData["errorMsg"] = "Role Deleted Successfully.!";
                }
                else
                {
                    TempData["errorMsg"] = "Role Deleted Successfully.!";
                }
                
                }
                return RedirectToAction("Index");
        }

    }
}