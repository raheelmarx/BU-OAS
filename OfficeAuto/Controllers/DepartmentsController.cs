using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeAuto.Models.DB;
using OfficeAuto.Models.ViewModels;

namespace OfficeAuto.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly OfficeAutoDBContext _context;

        public DepartmentsController(OfficeAutoDBContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var officeAutoDBContext = _context.Departments.Include(d => d.CampusId);
            return View(await officeAutoDBContext.ToListAsync());
        }

     
        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["CampusId"] = new SelectList(_context.Campuses, "Id", "Name");
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Departments dept = new Departments();
                dept.Name = model.Name;
                dept.DeptCode = model.DeptCode;
                dept.CampusId= model.CampusId;
                _context.Departments.Add(dept);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CampusId"] = new SelectList(_context.Campuses, "Id", "Name");
            return View(model);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departments = await _context.Departments.FindAsync(id);
            if (departments == null)
            {
                return NotFound();
            }
            DepartmentViewModel model;
            model = new DepartmentViewModel
            {
                Id = departments.Id,
                Name = departments.Name,
                DeptCode = departments.DeptCode,
                CampusId = departments.CampusId
            };
            ViewData["CampusId"] = new SelectList(_context.Campuses, "Id", "Name");
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Departments dept = new Departments();
                    dept.Id = model.Id;
                    dept.Name = model.Name;
                    dept.DeptCode = model.DeptCode;
                    dept.CampusId = model.CampusId;
                    _context.Update(dept);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentsExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CampusId"] = new SelectList(_context.Campuses, "Id", "Name");
            return View(model);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var departments = await _context.Departments.FindAsync(id);
            if (departments == null)
            {
                return NotFound();
            }
            _context.Departments.Remove(departments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentsExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
