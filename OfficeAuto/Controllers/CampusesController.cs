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
    //[Authorize]
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
            //await _context.Campuses.ToListAsync()
            return View();
        }

      

        // GET: Campuses/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CampusViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Campuses camp = new Campuses();
                //camp.Name = model.Name;
                //camp.CampusCode = model.CampusCode;
                //_context.Campuses.Add(camp);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Campuses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var campuses = await _context.Campuses.FindAsync(id);
            //if (campuses == null)
            //{
            //    return NotFound();
            //}
            //CampusViewModel model;
            //model = new CampusViewModel
            //{
            //    Id = campuses.Id,
            //    Name = campuses.Name,
            //    CampusCode = campuses.CampusCode,
            //};
            //model
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CampusViewModel model)
        {
            if (ModelState.IsValid)
            {
                //try
                //{
                //    Campuses camp = new Campuses();
                //    camp.Id = model.Id;
                //    camp.Name = model.Name;
                //    camp.CampusCode = model.CampusCode;
                //    _context.Update(camp);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!CampusesExists(model.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Campuses/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
