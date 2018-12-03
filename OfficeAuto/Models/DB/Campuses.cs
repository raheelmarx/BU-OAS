using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class Campuses
    {
        public Campuses()
        {
            Departments = new HashSet<Departments>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CampusCode { get; set; }

        public ICollection<Departments> Departments { get; set; }
    }
}
