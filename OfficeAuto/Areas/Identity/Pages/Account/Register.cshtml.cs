using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using OfficeAuto.Data;
using OfficeAuto.Models.DB;

namespace OfficeAuto.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly OfficeAutoDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, OfficeAutoDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Required]
            public string Title { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string Address { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string Province{ get; set; }
            [Required]
            public string Country { get; set; }

            [Required]
            [Display(Name = "Campus")]
            public int CampusId { get; set; }
            
            [Required]
            [Display(Name = "Department")]
            public int DeptId { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string RoleId { get; set; }
            
            [Display(Name = "Status")]
            public int? IsActive { get; set; } = 0;
           
        }

        public void OnGet(string returnUrl = null)
        {
            
            ViewData["Campuses"] = new SelectList(_context.Campuses, "Id", "Name");
            ViewData["Departments"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };

                user.Title = Input.Title;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Address = Input.Address;
                user.City = Input.City;
                user.Province = Input.Province;
                user.Country = Input.Country;
                user.DeptId = Input.DeptId;
                user.CampusId = Input.CampusId;
                user.IsActive = 0;

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Input.RoleId);
                     //await _userManager.AddToRoleAsync(user.Id, Input.RoleId);
                    _logger.LogInformation("User created a new account with password.");

                  /*  var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    */
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["Campuses"] = new SelectList(_context.Campuses, "Id", "Name");
            ViewData["Departments"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["Roles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
