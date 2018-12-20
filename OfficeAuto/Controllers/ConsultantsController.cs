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
    public class ConsultantsController : Controller
    {
        private readonly OfficeAutoDBContext _context;

        public ConsultantsController(OfficeAutoDBContext context)
        {
            _context = context;
        }

        // GET: Consultants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Consultants.ToListAsync());
        }

        // GET: Consultants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultants = await _context.Consultants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consultants == null)
            {
                return NotFound();
            }

            return View(consultants);
        }

        // GET: Consultants/Create
        public IActionResult Create()
        {
            ViewData["DepartmentsList"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsultantViewModel model)
        {
            if (ModelState.IsValid)
            {
                Consultants consul = new Consultants();
                consul.Name = model.Name;
                consul.Phone = model.Phone;
                consul.Email = model.Email;
                consul.Address = model.Address;
                consul.DeptId = model.DeptId;
                consul.Status = model.Status;
                _context.Consultants.Add(consul);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentsList"] = new SelectList(_context.Departments, "Id", "Name");
            return View(model);
        }

        // GET: Consultants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultant = await _context.Consultants.FindAsync(id);
            if (consultant == null)
            {
                return NotFound();
            }
            ConsultantViewModel model;
            model = new ConsultantViewModel
            {
                Id = consultant.Id,
                Name = consultant.Name,
                Phone = consultant.Phone,
                Email = consultant.Email,
                Address = consultant.Address,
                DeptId = consultant.DeptId,
                Status = consultant.Status
            };
            ViewData["DepartmentsList"] = new SelectList(_context.Departments, "Id", "Name");
            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ConsultantViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    Consultants consul = new Consultants();
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
                    if (!ConsultantsExists(model.Id))
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
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var consultants = await _context.Consultants.FindAsync(id);
            if (consultants == null)
            {
                return NotFound();
            }
            _context.Consultants.Remove(consultants);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultantsExists(int id)
        {
            return _context.Consultants.Any(e => e.Id == id);
        }
    }
}
