using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class Case
    {
        public Case()
        {
            Minutes = new HashSet<Minutes>();
            MinutesAssignedRelease = new HashSet<MinutesAssignedRelease>();
        }

        public long Id { get; set; }
        public string CaseNumber { get; set; }
        public string CaseTitle { get; set; }
        public DateTime? DateCreated { get; set; }
        public short Status { get; set; }

        public ICollection<Minutes> Minutes { get; set; }
        public ICollection<MinutesAssignedRelease> MinutesAssignedRelease { get; set; }
    }
}
