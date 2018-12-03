using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeAuto.Models.DB;
using System.Drawing;
using Microsoft.Extensions.Logging;
using OfficeAuto.Models;
using Microsoft.Extensions.Configuration;
using OfficeAuto.Helpers;
using OfficeAuto.Models.ViewModels;
using OfficeAuto.Data;

namespace OfficeAuto.Controllers
{
    public class MinutesController : Controller
    {
       
        private readonly OfficeAutoDBContext _context;

        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;

        public MinutesController(OfficeAutoDBContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: Minutes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Minutes.ToListAsync());
        }

        // GET: Minutes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var minutes = await _context.Minutes
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewBag.HtmlData = minutes.Description;

            var refdocs =  _context.ReferenceDoc.Where(t => t.MinuteId == minutes.Id).ToList();

            List<FileStreamResult> fileStreamResult = new List<FileStreamResult>();

            foreach(var doc in refdocs)
            {
                MemoryStream ms = new MemoryStream(doc.RefFile);

                var filestream = new FileStreamResult(ms, doc.ContentType);

                fileStreamResult.Add(filestream);
            }


            ViewBag.ReferenceDocs = refdocs;
            ViewBag.FileStreamResults = fileStreamResult;
            if (minutes == null)
            {
                return NotFound();
            }

            return View(minutes);
        }

        // GET: Minutes/Create
        public IActionResult Create(long caseId)
        {
            var @case = _context.Case.Where(x => x.Id == caseId && x.Status != 1).FirstOrDefault();
            if (@case == null)
            {
                return NotFound();
            }
            ViewBag.CaseId = caseId;
            ViewBag.CaseNumber = @case.CaseNumber;
            return View();
        }

        // POST: Minutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MinuteNumber,MinuteTitle,Description,CaseId,CaseTitle, Flag, Access")] CaseViewModel caseViewModel, IList<IFormFile> files)//,CreatedBy,UpdatedBy,DateUpdated,DateCreated,Status
        {

            string dbConn2 = _configuration.GetValue<string>("DocURL:FileServer");

            IFormFile uploadedImage = files.FirstOrDefault();

            var userid = _userManager.GetUserId(HttpContext.User);

            var username = _userManager.GetUserName(HttpContext.User);

            var @case = _context.Case.Where(x => x.Id == caseViewModel.Id && x.Status != 1).FirstOrDefault();
            if (@case == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                long minnum = MinuteNumberGenerator(@case.Id);

                Minutes minutes = new Minutes() {
                    Description = caseViewModel.Description,
                    MinuteNumber = minnum.ToString(),
                    MinuteTitle = caseViewModel.MinuteTitle,
                    CaseId = caseViewModel.Id,
                    CreatedBy = userid,
                DateCreated = DateTime.Now,
                Status = (short)1, };
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

                //foreach (var file in files)
                //{
                //    string fileServer = _configuration.GetValue<string>("DocURL:FileServer");
                //    if (file != null && file.Length > 0)
                //    {
                //        //var path = Path.Combine(
                //        //            fileServer, "wwwroot\\ReferenceDocs\\Cases\\" + @case.Id + "\\" + minutes.MinuteNumber);
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

                //        // adding uploaded file information to database
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

                //foreach (var file in files)
                //{
                //    if (file.Length > 0)
                //    {
                //        using (var ms = new MemoryStream())
                //        {
                //            file.CopyTo(ms);
                //            var fileBytes = ms.ToArray();
                //            //string s = Convert.ToBase64String(fileBytes);
                //            // act on the Base64 data
                //            ReferenceDoc imageEntity = new ReferenceDoc()
                //            {
                //                RefTitle = file.FileName,
                //                RefFile = fileBytes,
                //                DateCreated = DateTime.Now,
                //                MinuteId = minutes.Id,
                //                AddedBy = userid,
                //                ContentType = file.ContentType
                //            };

                //            _context.Add(imageEntity);
                //            await _context.SaveChangesAsync();
                //        }
                //    }
                //}
                return RedirectToAction("Index", "Cases");
            }
            return View(caseViewModel);
        }

        // GET: Minutes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var minutes = await _context.Minutes.FindAsync(id);
            if (minutes == null)
            {
                return NotFound();
            }
            //ViewBag.HtmlData = minutes.Description;

            var refdocs = _context.ReferenceDoc.Where(t => t.MinuteId == minutes.Id).ToList();
            List<FileStreamResult> fileStreamResult = new List<FileStreamResult>();
            foreach (var doc in refdocs)
            {
                MemoryStream ms = new MemoryStream(doc.RefFile);

                var filestream = new FileStreamResult(ms, doc.ContentType);

                fileStreamResult.Add(filestream);
            }
            ViewBag.ReferenceDocs = refdocs;
            ViewBag.FileStreamResults = fileStreamResult;
            return View(minutes);
        }

        // POST: Minutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,MinuteNumber,MinuteTitle,Description,Status")] Minutes minutes, IList<IFormFile> files)/*CreatedBy,UpdatedBy,DateUpdated,DateCreated,*/
        {
            IFormFile uploadedImage = files.FirstOrDefault();

            var userid = _userManager.GetUserId(HttpContext.User);
            var username = _userManager.GetUserName(HttpContext.User);

            if (id != minutes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    minutes.UpdatedBy = userid;
                    minutes.DateUpdated = DateTime.Now;
                    _context.Update(minutes);
                    await _context.SaveChangesAsync();

                    foreach (var file in files)
                    {


                        //Code to upload file in location
                        if (file != null && file.Length > 0)
                        {
                            var path = Path.Combine(
                                        Directory.GetCurrentDirectory(), "wwwroot\\ReferenceDocs\\Cases\\"+ minutes.Id,
                                        file.FileName);
                            if (!Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path);
                            }
                            //employeeProfileDTO.Photo = "\\images\\EmployeeProfile\\" + file.FileName;
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            //ReferenceDoc imageEntity = new ReferenceDoc()
                            //{
                            //    RefTitle = file.FileName,
                            //    RefFile = fileBytes,
                            //    DateCreated = DateTime.Now,
                            //    MinuteId = minutes.Id,
                            //    AddedBy = userid,
                            //    ContentType = file.ContentType
                            //};

                            //_context.Add(imageEntity);
                            //await _context.SaveChangesAsync();
                        }



                        //Code to save file content in Database
                        //if (file.Length > 0)
                        //{
                        //    using (var ms = new MemoryStream())
                        //    {
                        //        file.CopyTo(ms);
                        //        var fileBytes = ms.ToArray();
                        //        //string s = Convert.ToBase64String(fileBytes);
                        //        // act on the Base64 data
                        //        ReferenceDoc imageEntity = new ReferenceDoc()
                        //        {
                        //            RefTitle = file.FileName,
                        //            RefFile = fileBytes,
                        //            DateCreated = DateTime.Now,
                        //            MinuteId = minutes.Id,
                        //            AddedBy = userid,
                        //            ContentType = file.ContentType
                        //        };

                        //        _context.Add(imageEntity);
                        //        await _context.SaveChangesAsync();
                        //    }
                        //}
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MinutesExists(minutes.Id))
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
            return View(minutes);
        }

        // GET: Minutes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var minutes = await _context.Minutes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (minutes == null)
            {
                return NotFound();
            }

            return View(minutes);
        }

        // POST: Minutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var minutes = await _context.Minutes.FindAsync(id);
            _context.Minutes.Remove(minutes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MinutesExists(long id)
        {
            return _context.Minutes.Any(e => e.Id == id);
        }


        [HttpGet]
        public IActionResult ReferenceUpload()
        {
            using (OfficeAutoDBContext dbContext = new OfficeAutoDBContext())
            {
                List<long> iamgeIds = dbContext.ReferenceDoc.Select(m => m.Id).ToList();
                return View(iamgeIds);
            }
        }

        [HttpPost]
        public IActionResult ReferenceUpload(IList<IFormFile> files)
        {
            IFormFile uploadedImage = files.FirstOrDefault();

            var userid = _userManager.GetUserId(HttpContext.User);
            var username = _userManager.GetUserName(HttpContext.User);

            if (uploadedImage == null || uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                using (OfficeAutoDBContext dbContext = new OfficeAutoDBContext())
                {
                    MemoryStream ms = new MemoryStream();
                    uploadedImage.OpenReadStream().CopyTo(ms);

                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                    ReferenceDoc imageEntity = new ReferenceDoc()
                    {
                        Id = 1,
                        RefTitle = uploadedImage.Name,
                        RefFile = ms.ToArray(),
                        DateCreated = DateTime.Now,
                        MinuteId = image.Height,
                        AddedBy = userid
                    };

                    dbContext.ReferenceDoc.Add(imageEntity);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public FileStreamResult Viewfile(double id)
        {
            using (OfficeAutoDBContext dbContext = new OfficeAutoDBContext())
            {
                ReferenceDoc referenceDoc = dbContext.ReferenceDoc.FirstOrDefault(m => m.Id == id);

                MemoryStream ms = new MemoryStream(referenceDoc.RefFile);

                return new FileStreamResult(ms, referenceDoc.ContentType);
            }
        }

        public long MinuteNumberGenerator(long caseId)
        {
            var latestminute = _context.Minutes.Where(x => x.CaseId == caseId).LastOrDefault();
            if (latestminute != null)
            {
                return latestminute.Id + 1;
            }
            return 1;
        }

    }
}
