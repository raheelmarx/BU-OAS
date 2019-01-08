using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeAuto.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int DeptId { get; set; }
        public int CampusId { get; set; }
        public string City{ get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public int? IsActive { get; set; } = 0;
    }
}
