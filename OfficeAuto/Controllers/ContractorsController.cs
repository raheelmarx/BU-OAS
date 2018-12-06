using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeAuto.Models.DB;

namespace OfficeAuto.Controllers
{
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

        // GET: Contractors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractors = await _context.Contractors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contractors == null)
            {
                return NotFound();
            }

            return View(contractors);
        }

        // GET: Contractors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,Email,Address,DeptId,Status")] Contractors contractors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contractors);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contractors);
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
            return View(contractors);
        }

        // POST: Contractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Email,Address,DeptId,Status")] Contractors contractors)
        {
            if (id != contractors.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contractors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractorsExists(contractors.Id))
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
            return View(contractors);
        }

        // GET: Contractors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contractors = await _context.Contractors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contractors == null)
            {
                return NotFound();
            }

            return View(contractors);
        }

        // POST: Contractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contractors = await _context.Contractors.FindAsync(id);
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
