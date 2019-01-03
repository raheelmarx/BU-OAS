using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class MinutesAssignedDraft
    {
        public long Id { get; set; }
        public long? MinuteId { get; set; }
        public string AssignedFromUserId { get; set; }
        public string AssignedToUserId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool? ResponseReceived { get; set; }

        public Minutes Minute { get; set; }
    }
}
