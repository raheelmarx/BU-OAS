using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class ReferenceDoc
    {
        public long Id { get; set; }
        public string RefTitle { get; set; }
        public byte[] RefFile { get; set; }
        public string DocPath { get; set; }
        public string Flag { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? MinuteId { get; set; }
        public string AddedBy { get; set; }
        public string ContentType { get; set; }
        public string Access { get; set; }

        public Minutes Minute { get; set; }
    }
}
