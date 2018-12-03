using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class Contractors
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int DeptId { get; set; }
        public int? Status { get; set; }
    }
}
