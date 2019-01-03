using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OfficeAuto.Models.ViewModels
{
    public class CaseViewModel
    {
        //CaseTitle,MinuteNumber,MinuteTitle,Description
        public long Id { get; set; }
        public string CaseNumber { get; set; }
        public string CaseTitle { get; set; }
        public string MinuteNumber { get; set; }
        public long MinuteId { get; set; }
        public string MinuteTitle { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Flag { get; set; }
        public string Access { get; set; }
        public List<UserViewModel> AssignedTo { get; set; }
        public string DocIds { get; set; }

    }
}
