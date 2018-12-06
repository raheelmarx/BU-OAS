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
    public class CampusesController : Controller
    {
        private readonly OfficeAutoDBContext _context;

        public CampusesController(OfficeAutoDBContext context)
        {
            _context = context;
        }

        // GET: Campuses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Campuses.ToListAsync());
        }

        // GET: Campuses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campuses = await _context.Campuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campuses == null)
            {
                return NotFound();
            }

            return View(campuses);
        }

        // GET: Campuses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Campuses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CampusCode")] Campuses campuses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(campuses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(campuses);
        }

        // GET: Campuses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campuses = await _context.Campuses.FindAsync(id);
            if (campuses == null)
            {
                return NotFound();
            }
            return View(campuses);
        }

        // POST: Campuses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CampusCode")] Campuses campuses)
        {
            if (id != campuses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campuses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampusesExists(campuses.Id))
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
            return View(campuses);
        }

        // GET: Campuses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campuses = await _context.Campuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campuses == null)
            {
                return NotFound();
            }

            return View(campuses);
        }

        // POST: Campuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var campuses = await _context.Campuses.FindAsync(id);
            _context.Campuses.Remove(campuses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampusesExists(int id)
        {
            return _context.Campuses.Any(e => e.Id == id);
        }
    }
}
