using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeAuto.Data;
using OfficeAuto.Helpers;
using OfficeAuto.Models.DB;
using OfficeAuto.Models.ViewModels;

namespace OfficeAuto.Controllers
{
   
    public class CasesController : Controller
    {
        private readonly OfficeAutoDBContext _context;

        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;

        public CasesController(OfficeAutoDBContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }


        //public CasesController(OfficeAutoDBContext context)
        //{
        //    _context = context;
        //}

        // GET: Cases
        public async Task<IActionResult> Index()
        {
            return View(await _context.Case.ToListAsync());
        }

        // GET: Cases/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }

        // GET: Cases/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseTitle,MinuteNumber,MinuteTitle,Description, Flag, Access")] CaseViewModel caseViewModel, IList<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                string caseNum = CaseNumberGenerator().ToString();
                Case @case = new Case()
                {
                    CaseTitle = caseViewModel.CaseTitle,
                    DateCreated = DateTime.Now,
                    Status = (short)caseViewModel.Status,
                    CaseNumber = caseNum
                };
                _context.Add(@case);
                _context.SaveChanges();

                //@case.CaseNumber = @case.Id.ToString();// Call the method to generate CaseNumber
                //_context.Update(@case);
                //await _context.SaveChangesAsync();

                #region Minute creation under case
                string dbConn2 = _configuration.GetValue<string>("DocURL:FileServer");

                IFormFile uploadedImage = files.FirstOrDefault();

                var userid = _userManager.GetUserId(HttpContext.User);
                var username = _userManager.GetUserName(HttpContext.User);

                long minnum =MinuteNumberGenerator(@case.Id);

                Minutes minutes = new Minutes() {
                    MinuteNumber = minnum.ToString(),
                    MinuteTitle = caseViewModel.MinuteTitle,
                    Description = caseViewModel.Description,
                    CaseId = @case.Id,
                CreatedBy = userid,
                DateCreated = DateTime.Now,
                Status = 1,
            };
                    _context.Add(minutes);
                    _context.SaveChanges();

                    #region Reference Documents creation under Minute
                    foreach (var file in files)
                    {
                    
                        string fileServer = _configuration.GetValue<string>("DocURL:FileServer");
                        if (file != null && file.Length > 0)
                        {
                        Flag flagvalue = (Flag)int.Parse(caseViewModel.Flag);

                        var path = Path.Combine(
                                        fileServer, "wwwroot\\ReferenceDocs\\Cases\\"+ @case.CaseNumber +"\\"+ caseViewModel.MinuteNumber + "\\" + flagvalue.ToString());
                            if (!Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }

                        var path2 = Path.Combine(path, file.FileName);
                            using (var stream = new FileStream(path2, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        ReferenceDoc imageEntity = new ReferenceDoc()
                        {
                            RefTitle = file.FileName,              
                            DateCreated = DateTime.Now,
                            MinuteId = minutes.Id,
                            AddedBy = userid,
                            ContentType = file.ContentType,
                            DocPath = path2,
                            Flag = caseViewModel.Flag.ToString(),
                            Access = caseViewModel.Access.ToString()
                        };

                        _context.Add(imageEntity);
                        await _context.SaveChangesAsync();

                    }
                    }
                    
                    #endregion
                    return RedirectToAction(nameof(Index));
                
                #endregion
            }
            return View(caseViewModel);
        }

        // GET: Cases/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = _context.Case.Where(x => x.Id == id).FirstOrDefault();
            if (@case == null)
            {
                return NotFound();
            }
            if (@case.Status != 0)
            {
                return  RedirectToAction("Create", "Minutes", new { caseId = @case.Id });
            }
            var minute =  _context.Minutes.Where(x => x.CaseId == id).FirstOrDefault();
            var refdocs = _context.ReferenceDoc.Where(x => x.MinuteId == minute.Id).ToList();
            CaseViewModel caseViewModel = new CaseViewModel()
            {
                CaseTitle = @case.CaseTitle,
                MinuteNumber = minute.MinuteNumber,
                MinuteTitle = minute.MinuteTitle,
                Description = minute.Description,
                Status = (int)@case.Status
            };
            ViewBag.RefDocs = refdocs;
            ViewBag.CaseId = id;
           
            return View(caseViewModel);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CaseTitle,DateCreated,Status,MinuteNumber,MinuteTitle,Description")] CaseViewModel caseViewModel, IList<IFormFile> files)
        {
            if (id != caseViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var @case = _context.Case.Where(x => x.Id == id && x.Status == 0).FirstOrDefault();
                    if (@case == null)
                    {
                        return NotFound();
                    }

                    // updating case
                    @case.Status = (short)caseViewModel.Status;
                    @case.CaseTitle = caseViewModel.CaseTitle;
                    _context.Update(@case);
                    await _context.SaveChangesAsync();

                    IFormFile uploadedImage = files.FirstOrDefault();

                    var userid = _userManager.GetUserId(HttpContext.User);
                    var username = _userManager.GetUserName(HttpContext.User);
                    var minutes = _context.Minutes.Where(x => x.CaseId == id).FirstOrDefault();


                    // Taking Current History of Minutes
                    var oldMinutes = new MinutesHistory()
                    {
                        MinuteId = minutes.Id,
                        MinuteNumber = minutes.MinuteNumber,
                        Description = minutes.Description,
                        DateCreated = minutes.DateCreated,
                        DateUpdated = DateTime.Now,
                        UpdatedBy = userid
                    };
                    _context.Add(oldMinutes);
                    _context.SaveChanges();
                    //

                    //Updating the minute to newest edit

                    minutes.UpdatedBy = userid;
                            minutes.DateUpdated = DateTime.Now;
                            minutes.Description = caseViewModel.Description;
                            _context.Update(minutes);
                            await _context.SaveChangesAsync();


                    // Uploading more files if added
                            foreach (var file in files)
                            {
                        string fileServer = _configuration.GetValue<string>("DocURL:FileServer");
                        if (file != null && file.Length > 0)
                        {
                            //var path = Path.Combine(
                            //            fileServer, "wwwroot\\ReferenceDocs\\Cases\\" + @case.Id + "\\" + minutes.MinuteNumber);
                            Flag flagvalue = (Flag)int.Parse(caseViewModel.Flag);

                            var path = Path.Combine(
                                            fileServer, "wwwroot\\ReferenceDocs\\Cases\\" + @case.CaseNumber + "\\" + caseViewModel.MinuteNumber + "\\" + flagvalue.ToString());

                            if (!Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }

                            var path2 = Path.Combine(path, file.FileName);
                            using (var stream = new FileStream(path2, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // adding uploaded file information to database
                            ReferenceDoc imageEntity = new ReferenceDoc()
                            {
                                RefTitle = file.FileName,
                                DateCreated = DateTime.Now,
                                MinuteId = minutes.Id,
                                AddedBy = userid,
                                ContentType = file.ContentType,
                                DocPath = path2,
                                Flag = caseViewModel.Flag.ToString(),
                                Access = caseViewModel.Access.ToString()
                            };

                            _context.Add(imageEntity);
                            await _context.SaveChangesAsync();
                        }
                        }
                        
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(caseViewModel.Id))
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
            return View(caseViewModel);
        }

        // GET: Cases/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Case
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var @case = await _context.Case.FindAsync(id);
            _context.Case.Remove(@case);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaseExists(long id)
        {
            return _context.Case.Any(e => e.Id == id);
        }

        public string CaseNumberGenerator()
        {
            var latestcase =  _context.Case.LastOrDefault();
            var campus =  _context.Campuses.Where(x=>x.Id == 1).FirstOrDefault();
            var dept =  _context.Departments.Where(x=>x.CampusId ==campus.Id && x.Id == 1).FirstOrDefault();
            long newcase = latestcase.Id + 1;
            string caseno = campus.CampusCode+"\\" + dept.DeptCode+"\\"+DateTime.Now.Year+"\\" + newcase.ToString();
            return caseno;
        }

        public long MinuteNumberGenerator(long caseId)
        {
            var latestminute =  _context.Minutes.Where(x=>x.CaseId == caseId).LastOrDefault();
            if (latestminute != null)
            {
                return latestminute.Id + 1;
            }
            return 1;
        }

    }
}
