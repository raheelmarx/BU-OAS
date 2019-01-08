using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class Departments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DeptCode { get; set; }
        public int CampusId { get; set; }
        public Campuses Campus { get; set; }
    }
}
