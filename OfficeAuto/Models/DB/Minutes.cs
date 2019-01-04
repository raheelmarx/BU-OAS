using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class Minutes
    {
        public Minutes()
        {
            MinutesAssignedDraft = new HashSet<MinutesAssignedDraft>();
            MinutesHistory = new HashSet<MinutesHistory>();
            ReferenceDoc = new HashSet<ReferenceDoc>();
        }

        public long Id { get; set; }
        public string MinuteNumber { get; set; }
        public string MinuteTitle { get; set; }
        public long? CaseId { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateCreated { get; set; }
        public short? Status { get; set; }

        public Case Case { get; set; }
        public ICollection<MinutesAssignedDraft> MinutesAssignedDraft { get; set; }
        public ICollection<MinutesHistory> MinutesHistory { get; set; }
        public ICollection<ReferenceDoc> ReferenceDoc { get; set; }
    }
}
