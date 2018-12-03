using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class MinutesAssidnedDraft
    {
        public long Id { get; set; }
        public long? MinuteId { get; set; }
        public string Assigner { get; set; }
        public string Assignee { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? ResponseReceived { get; set; }

        public Minutes Minute { get; set; }
    }
}
