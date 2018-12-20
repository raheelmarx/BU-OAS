using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OfficeAuto.Data;

namespace OfficeAuto.Areas.Identity.Pages.Account.Manage
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UsersModel> _logger;

        public UsersModel(
            UserManager<ApplicationUser> userManager,
            ILogger<UsersModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string Province { get; set; }
            public string Country { get; set; }

            [Display(Name = "Campus")]
            public int CampusId { get; set; }

            [Display(Name = "Department")]
            public int DeptId { get; set; }

            [Display(Name = "Role")]
            public string RoleId { get; set; }

            public int? IsActive { get; set; } = 0;
        }

       
    }
}