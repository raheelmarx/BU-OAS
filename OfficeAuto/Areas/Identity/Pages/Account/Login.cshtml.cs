using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OfficeAuto.Data;
using Microsoft.Extensions.Configuration;
using OfficeAuto.Helpers;
using Microsoft.AspNetCore.Http;

namespace OfficeAuto.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private string Attempts="0";
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private IConfiguration _configuration;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["ReCaptchaKey"] = _configuration.GetSection("GoogleReCaptcha:key").Value;
            var val=HttpContext.Session.GetString(Attempts);
            if (val == null)
            {
                HttpContext.Session.SetString(Attempts, "0");
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
           // int at= 0;
           // var old_val = HttpContext.Session.GetString(Attempts);
            //ViewData["ReCaptchaKey"] = _configuration.GetSection("GoogleReCaptcha:key").Value;


            if (ModelState.IsValid)
            {
                //if (Convert.ToInt32(TempData["Attempts"]) > 3) {

                //    if (!Captcha.ReCaptchaPassed(Request.Form["g-recaptcha-response"],_configuration.GetSection("GoogleReCaptcha:secret").Value))
                //    {
                //        ModelState.AddModelError(string.Empty, "You failed the CAPTCHA,Please enter correct values.");
                //        return Page();
                //    }
                //}
                //// This doesn't count login failures towards account lockout
                ////var count = _userManager.GetAccessFailedCountAsync();
                //// To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                //if (result.Succeeded)
                //{
                //    _logger.LogInformation("User logged in.");
                //    return LocalRedirect(returnUrl);
                //}
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //}
                //if (result.IsLockedOut)
                //{
                //    _logger.LogWarning("User account locked out.");
                //    ModelState.AddModelError(string.Empty, "Your account locked out,please contract administrator.");
                //    return Page();
                //    //return RedirectToPage("./Lockout");
                //}
                //else
                //{
                //    at = Convert.ToInt32(old_val) + 1;
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //    return Page();
                //}
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                return LocalRedirect(returnUrl);
            }

         //   HttpContext.Session.SetString(Attempts, at.ToString()); 

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
