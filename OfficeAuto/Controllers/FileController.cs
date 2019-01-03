using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeAuto.Models.DB;
using Microsoft.AspNetCore.Identity;
using OfficeAuto.Data;
using OfficeAuto.Helpers;
using OfficeAuto.Models.ViewModels;
using System.Linq;
using System;

namespace OfficeAuto.Controllers
{
    public class FileController : Controller
    {
        private readonly OfficeAutoDBContext _context;

        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;

        public FileController(OfficeAutoDBContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([Bind("Id,CaseTitle,MinuteNumber,MinuteId,MinuteTitle,Description, Flag, Access")] CaseViewModel caseViewModel,IList<IFormFile> files, long? id)
        {
            var userid = _userManager.GetUserId(HttpContext.User);

            var @case = _context.Case.Where(x => x.Id == id).FirstOrDefault();
            if (@case == null)
            {
                return NotFound();
            }
            //if (@case.Status != 1)
            //{
            //    return RedirectToAction("Create", "Minutes", new { caseId = @case.Id });
            //}


            foreach (var file in files)
            {

                string fileServer = _configuration.GetValue<string>("DocURL:FileServer");

                //string fileServer = Directory.GetCurrentDirectory();
                if (file != null && file.Length > 0)
                {
                    Flag flagvalue = (Flag)int.Parse(caseViewModel.Flag);

                    var path = Path.Combine(
                                    fileServer, "wwwroot\\ReferenceDocs\\Cases\\" + @case.Id + "\\" + caseViewModel.MinuteId + "\\" + flagvalue.ToString());
                    if (!Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    var path2 = Path.Combine(path, file.FileName);
                    using (var stream = new FileStream(path2, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    //var javascriptPath =  Path.Combine(
                    //               "http:\\\\\\\\file:\\\\\\\\\\\\"+ fileServer+"\\\\\\", "wwwroot\\\\\\\\ReferenceDocs\\\\\\\\Cases\\\\\\\\" + @case.Id + "\\\\\\\\" + caseViewModel.MinuteId + "\\\\\\\\" + flagvalue.ToString()+"\\\\\\\\"+ file.FileName);



                    var javascriptPath = "ReferenceDocs/Cases/" + @case.Id + "/" + caseViewModel.MinuteId + "/" + flagvalue.ToString() + "/" + file.FileName;

                    ReferenceDoc imageEntity = new ReferenceDoc()
                    {
                        RefTitle = file.FileName,
                        DateCreated = DateTime.Now,
                        MinuteId = caseViewModel.MinuteId,
                        CaseId = caseViewModel.Id,
                        AddedBy = userid,
                        ContentType = file.ContentType,
                        DocPath = javascriptPath,
                        Flag = caseViewModel.Flag.ToString(),
                        Access = caseViewModel.Access.ToString()
                    };

                    _context.Add(imageEntity);
                    await _context.SaveChangesAsync();

                }
            }




            //if (file == null || file.Length == 0)
            //    return Content("file not selected");

            //var path = Path.Combine(
            //            Directory.GetCurrentDirectory(), "wwwroot\\Cvs",
            //            file.FileName);

            //using (var stream = new FileStream(path, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}
            return RedirectToAction("Create", "Cases", new { CaseId = @case.Id, MinuteId  = caseViewModel.MinuteId});
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot\\Cvs", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}