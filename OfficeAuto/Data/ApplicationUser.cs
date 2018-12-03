﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeAuto.Data
{
    public class ApplicationUser: IdentityUser
    {

       public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int DeptId { get; set; }
    }
}
