using System.ComponentModel.DataAnnotations;


namespace OfficeAuto.Models.ViewModels
{
    public class CampusViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Campus Code")]
        public string CampusCode { get; set; }
    }

    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Department Code")]
        public string DeptCode { get; set; }
        
        [Required]
        [Display(Name = "Campus")]
        public int CampusId { get; set; }
    }

    public class ConsultantViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DeptId { get; set; }

        public int? Status { get; set; }

    }

    public class ContractorViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DeptId { get; set; }

        public int? Status { get; set; }

    }
}