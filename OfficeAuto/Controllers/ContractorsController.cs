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
    public class ContractorsController : Controller
    {
        private readonly OfficeAutoDBContext _context;

        public ContractorsController(OfficeAutoDBContext context)
        {
            _context = context;
        }

        // GET: Contractors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contractors.ToListAsync());
        }

        // GET: Contractors/Create
        public IActionResult Create()
        {
            ViewData["DepartmentsList"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractorViewModel model)
        {
            if (ModelState.IsValid)
            {
                Contractors contr = new Contractors();
                contr.Name = model.Name;
                contr.Phone = model.Phone;
                contr.Email = model.Email;
                contr.Address = model.Address;
                contr.DeptId = model.DeptId;
                contr.Status = model.Status;
                _context.Contractors.Add(contr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentsList"] = new SelectList(_context.Departments, "Id", "Name");
            return View(model);
        }

        // GET: Contractors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractors = await _context.Contractors.FindAsync(id);
            if (contractors == null)
            {
                return NotFound();
            }
            ContractorViewModel model;
            model = new ContractorViewModel
            {
                Id = contractors.Id,
                Name = contractors.Name,
                Phone = contractors.Phone,
                Email = contractors.Email,
                Address = contractors.Address,
                DeptId = contractors.DeptId,
                Status = contractors.Status
            };
            ViewData["DepartmentsList"] = new SelectList(_context.Departments, "Id", "Name");
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContractorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Contractors consul = new Contractors();
                    consul.Id = model.Id;
                    consul.Name = model.Name;
                    consul.Phone = model.Phone;
                    consul.Email = model.Email;
                    consul.Address = model.Address;
                    consul.DeptId = model.DeptId;
                    consul.Status = model.Status;
                    _context.Update(consul);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractorsExists(model.Id))
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
            ViewData["DepartmentsList"] = new SelectList(_context.Departments, "Id", "Name");
            return View(model);
        }

        // GET: Contractors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contractors = await _context.Contractors.FindAsync(id);
            if (contractors == null)
            {
                return NotFound();
            }
            _context.Contractors.Remove(contractors);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractorsExists(int id)
        {
            return _context.Contractors.Any(e => e.Id == id);
        }
    }
}
