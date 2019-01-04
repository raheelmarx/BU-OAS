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
using Newtonsoft.Json;
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
        public async Task<IActionResult> Create(long? CaseId, long? MinuteId)
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            var username = _userManager.GetUserName(HttpContext.User);

            var @users = await _userManager.Users.Select(x=> new SelectListItem { Value = x.Id.ToString(), Text = x.Email }).ToListAsync();// Get Users On base of loggedIn User
            
            if (userid == null)
            {
                return NotFound();
            }
            CaseViewModel caseViewModel = new CaseViewModel();
            if (CaseId == null)
            {
                Case @case = new Case();
                _context.Add(@case);
                _context.SaveChanges();
                caseViewModel.Id = @case.Id;
                //ViewBag.CaseId = @case.Id;

                Minutes minutes = new Minutes()
                {
                    CaseId = @case.Id,
                    CreatedBy = userid
                };
                _context.Add(minutes);
                _context.SaveChanges();
                caseViewModel.MinuteId = minutes.Id;
                //ViewBag.MinuteId = minutes.Id;
            }
            else
            {
                caseViewModel.Id = (long)CaseId;
                caseViewModel.MinuteId = (long)MinuteId;
                //ViewBag.CaseId = id;
                var refdocs = _context.ReferenceDoc.Where(x => x.CaseId == CaseId).ToList();
                ViewBag.RefDocs = refdocs;
            }
           
            ViewBag.Users = @users;
            return View(caseViewModel);
        }

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CaseTitle,MinuteNumber,MinuteId,MinuteTitle,Description,DocIds, Flag, Access")] CaseViewModel caseViewModel, IList<IFormFile> files, IFormCollection collection)
        {
           // var caseId = collection["id"].ToString();
            if (caseViewModel.Id.ToString() == null)
            {
                if (ModelState.IsValid)
                {

                    var assignedToList = collection["AssignedTo"];

                    var documentSelected = collection["documentSelected"];

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
                    //string dbConn2 = _configuration.GetValue<string>("DocURL:FileServer");

                    //IFormFile uploadedImage = files.FirstOrDefault();

                    var userid = _userManager.GetUserId(HttpContext.User);
                    var username = _userManager.GetUserName(HttpContext.User);

                    //long minnum =MinuteNumberGenerator(@case.Id);

                    Minutes minutes = new Minutes()
                    {
                        MinuteNumber = "1",//minnum.ToString(),
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
                    //foreach (var file in files)
                    //{

                    //    string fileServer = _configuration.GetValue<string>("DocURL:FileServer");
                    //    if (file != null && file.Length > 0)
                    //    {
                    //        Flag flagvalue = (Flag)int.Parse(caseViewModel.Flag);

                    //        var path = Path.Combine(
                    //                        fileServer, "wwwroot\\ReferenceDocs\\Cases\\" + @case.CaseNumber + "\\" + caseViewModel.MinuteNumber + "\\" + flagvalue.ToString());
                    //        if (!Directory.Exists(path))
                    //        {
                    //            System.IO.Directory.CreateDirectory(path);
                    //        }

                    //        var path2 = Path.Combine(path, file.FileName);
                    //        using (var stream = new FileStream(path2, FileMode.Create))
                    //        {
                    //            await file.CopyToAsync(stream);
                    //        }
                    //        ReferenceDoc imageEntity = new ReferenceDoc()
                    //        {
                    //            RefTitle = file.FileName,
                    //            DateCreated = DateTime.Now,
                    //            MinuteId = minutes.Id,
                    //            AddedBy = userid,
                    //            ContentType = file.ContentType,
                    //            DocPath = path2,
                    //            Flag = caseViewModel.Flag.ToString(),
                    //            Access = caseViewModel.Access.ToString()
                    //        };

                    //        _context.Add(imageEntity);
                    //        await _context.SaveChangesAsync();

                    //    }
                    //}


                    //string[] docId2 = caseViewModel.DocIds.Split(',').ToArray();

                    //double[] docId3 = Array.ConvertAll(docId2, s => double.Parse(s));

                    //var docIdList = _context.ReferenceDoc.Where(x => docId3.Contains(x.Id)).ToList();
                    
                    //foreach(var m in docIdList)
                    //{
                    //    m.MinuteId = minutes.Id;
                    //    _context.Update(m);
                    //    await _context.SaveChangesAsync();
                    //}

                    foreach (var AssignedToUserId in assignedToList)
                    {
                        if (caseViewModel.Status == 1)
                        {
                            MinutesAssignedDraft minutesAssignedDraft = new MinutesAssignedDraft()
                            {
                                MinuteId = minutes.Id,
                                AssignedFromUserId = userid,
                                AssignedToUserId = AssignedToUserId,
                                DateCreated = DateTime.Now,
                                ResponseReceived = false
                            };
                            _context.Add(minutesAssignedDraft);
                            _context.SaveChanges();
                        }
                        else
                        {
                            MinutesAssignedRelease minutesAssignedRelease = new MinutesAssignedRelease()
                            {
                                CaseId = @case.Id,
                                AssignedFromUserId = userid,
                                AssignedToUserId = AssignedToUserId,
                                DateCreated = DateTime.Now,
                                ResponseReceived = false
                            };
                            _context.Add(minutesAssignedRelease);
                            _context.SaveChanges();
                        }
                    }


                    #endregion
                    return RedirectToAction(nameof(Index));

                    #endregion
                }
            }

            else
            {
                if (ModelState.IsValid)
                {

                    var assignedToList = collection["AssignedTo"];

                    string caseNum = CaseNumberGenerator().ToString();
                    var @case = _context.Case.Where(x => x.Id == caseViewModel.Id).FirstOrDefault();
                    if (@case == null)
                    {
                        return NotFound();
                    }
                    //Case @case = new Case()
                    //{
                    //    CaseTitle = caseViewModel.CaseTitle,
                    //    DateCreated = DateTime.Now,
                    //    Status = (short)caseViewModel.Status,
                    //    CaseNumber = caseNum
                    //};

                    @case.CaseTitle = caseViewModel.CaseTitle;
                    @case.DateCreated = DateTime.Now;
                    @case.Status = (short)caseViewModel.Status;
                    @case.CaseNumber = caseNum;
                    _context.Update(@case);
                    await _context.SaveChangesAsync();

                    //@case.CaseNumber = @case.Id.ToString();// Call the method to generate CaseNumber
                    //_context.Update(@case);
                    //await _context.SaveChangesAsync();

                    #region Minute creation under case
                   // string dbConn2 = _configuration.GetValue<string>("DocURL:FileServer");

                    IFormFile uploadedImage = files.FirstOrDefault();

                    var userid = _userManager.GetUserId(HttpContext.User);
                    var username = _userManager.GetUserName(HttpContext.User);

                    //long minnum =MinuteNumberGenerator(@case.Id);
                    var minute = _context.Minutes.Where(x => x.CaseId == caseViewModel.Id && x.Id== caseViewModel.MinuteId).FirstOrDefault();
                    //Minutes minutes = new Minutes()
                    //{
                    minute.MinuteNumber = "1";//minnum.ToString(),
                    minute.MinuteTitle = caseViewModel.MinuteTitle;
                    minute.Description = caseViewModel.Description;
                    //CaseId = @case.Id,
                    minute.CreatedBy = userid;
                    minute.DateCreated = DateTime.Now;
                    minute.Status = 1;
                    //};
                    _context.Update(minute);
                    _context.SaveChanges();

                    #region Reference Documents creation under Minute
                    //foreach (var file in files)
                    //{

                    //    string fileServer = _configuration.GetValue<string>("DocURL:FileServer");
                    //    if (file != null && file.Length > 0)
                    //    {
                    //        Flag flagvalue = (Flag)int.Parse(caseViewModel.Flag);

                    //        var path = Path.Combine(
                    //                        fileServer, "wwwroot\\ReferenceDocs\\Cases\\" + @case.CaseNumber + "\\" + caseViewModel.MinuteNumber + "\\" + flagvalue.ToString());
                    //        if (!Directory.Exists(path))
                    //        {
                    //            System.IO.Directory.CreateDirectory(path);
                    //        }

                    //        var path2 = Path.Combine(path, file.FileName);
                    //        using (var stream = new FileStream(path2, FileMode.Create))
                    //        {
                    //            await file.CopyToAsync(stream);
                    //        }
                    //        ReferenceDoc imageEntity = new ReferenceDoc()
                    //        {
                    //            RefTitle = file.FileName,
                    //            DateCreated = DateTime.Now,
                    //            MinuteId = minutes.Id,
                    //            AddedBy = userid,
                    //            ContentType = file.ContentType,
                    //            DocPath = path2,
                    //            Flag = caseViewModel.Flag.ToString(),
                    //            Access = caseViewModel.Access.ToString()
                    //        };

                    //        _context.Add(imageEntity);
                    //        await _context.SaveChangesAsync();

                    //    }
                    //}
                    #endregion
                    foreach (var AssignedToUserId in assignedToList)
                    {
                        if (caseViewModel.Status == 1)
                        {
                            MinutesAssignedDraft minutesAssignedDraft = new MinutesAssignedDraft()
                            {
                                MinuteId = minute.Id,
                                AssignedFromUserId = userid,
                                AssignedToUserId = AssignedToUserId,
                                DateCreated = DateTime.Now,
                                ResponseReceived = false
                            };
                            _context.Add(minutesAssignedDraft);
                            _context.SaveChanges();
                        }
                        else
                        {
                            MinutesAssignedRelease minutesAssignedRelease = new MinutesAssignedRelease()
                            {
                                CaseId = @case.Id,
                                AssignedFromUserId = userid,
                                AssignedToUserId = AssignedToUserId,
                                DateCreated = DateTime.Now,
                                ResponseReceived = false
                            };
                            _context.Add(minutesAssignedRelease);
                            _context.SaveChanges();
                        }
                    }


                   
                    return RedirectToAction(nameof(Index));

                    #endregion
                }
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
            if (@case.Status != 1)
            {
                return  RedirectToAction("Create", "Minutes", new { caseId = @case.Id });
            }
            var minute =  _context.Minutes.Where(x => x.CaseId == id).FirstOrDefault();
            var refdocs = _context.ReferenceDoc.Where(x => x.MinuteId == minute.Id).ToList();

            var assgnto = _context.MinutesAssignedDraft.Where(x => x.MinuteId == minute.Id).Select(x=>x.AssignedToUserId).ToList();
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

            var @users = await _userManager.Users.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Email }).ToListAsync();
            ViewBag.Users = @users;
            ViewBag.AssignedTo = assgnto;

            return View(caseViewModel);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CaseTitle,DateCreated,Status,MinuteNumber,MinuteTitle,Description,AssignedTo")] CaseViewModel caseViewModel, IList<IFormFile> files, IFormCollection collection)
        {
            if (id != caseViewModel.Id)
            {
                return NotFound();
            }
            var assignedToList = collection["AssignedTo"];
            if (ModelState.IsValid)
            {
                try
                {
                    var @case = _context.Case.Where(x => x.Id == id && x.Status == 1).FirstOrDefault();
                    if (@case == null)
                    {
                        return NotFound();
                    }

                    // updating case
                    @case.Status = (short)caseViewModel.Status;
                    @case.CaseTitle = caseViewModel.CaseTitle;
                    _context.Update(@case);
                    await _context.SaveChangesAsync();

                    //IFormFile uploadedImage = files.FirstOrDefault();

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

                    var assgnto = _context.MinutesAssignedDraft.Where(x => x.MinuteId == minutes.Id).ToList();

                    foreach (var AssignedToUserId in assignedToList)
                    {
                        if (assgnto.Any(x => x.AssignedToUserId == AssignedToUserId))
                        {
                            continue;
                        }
                        else
                        {
                            MinutesAssignedDraft minutesAssignedDraft = new MinutesAssignedDraft()
                            {
                                MinuteId = minutes.Id,
                                AssignedFromUserId = userid,
                                AssignedToUserId = AssignedToUserId,
                                DateCreated = DateTime.Now,
                                ResponseReceived = false
                            };
                            _context.Add(minutesAssignedDraft);
                            _context.SaveChanges();
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

        //public long MinuteNumberGenerator(long caseId)
        //{
        //    var latestminute =  _context.Minutes.Where(x=>x.CaseId == caseId).LastOrDefault();
        //    if (latestminute != null)
        //    {
        //        return latestminute.Id + 1;
        //    }
        //    return 1;
        //}

    }
}
