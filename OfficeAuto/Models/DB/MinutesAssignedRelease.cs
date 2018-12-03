using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class MinutesAssignedRelease
    {
        public long Id { get; set; }
        public long? CaseId { get; set; }
        public string Assigner { get; set; }
        public string Assignee { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? ResponseReceived { get; set; }

        public Case Case { get; set; }
    }
}
