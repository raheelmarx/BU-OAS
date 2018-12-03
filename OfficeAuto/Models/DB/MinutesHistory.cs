using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class MinutesHistory
    {
        public long Id { get; set; }
        public long? MinuteId { get; set; }
        public string MinuteNumber { get; set; }
        public string Description { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateCreated { get; set; }

        public Minutes Minute { get; set; }
    }
}
