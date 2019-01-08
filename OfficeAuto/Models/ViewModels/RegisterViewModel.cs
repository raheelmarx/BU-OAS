using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeAuto.Models.ViewModels
{
    public class RegisterViewModel
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
    public class AssignAOLAODViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        [Display(Name = "Users")]
        [Required]
        public string AOUserId{ get; set; }
        [DataType(DataType.Date)]
        public DateTime DateAssigned { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateExpired{ get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Status{ get; set; }
    }
}
