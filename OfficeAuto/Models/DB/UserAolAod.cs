using System;
using System.Collections.Generic;

namespace OfficeAuto.Models.DB
{
    public partial class UserAolAod
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string AoUserid { get; set; }
        public DateTime? DateAssigned { get; set; }
        public DateTime? DateExpired { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? Status { get; set; }
    }
}
