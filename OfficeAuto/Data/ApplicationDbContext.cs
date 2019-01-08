using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfficeAuto.Models.ViewModels;



namespace OfficeAuto.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<OfficeAuto.Data.ApplicationRole> ApplicationRole { get; set; }

        public DbSet<OfficeAuto.Models.ViewModels.RegisterViewModel> RegisterViewModel { get; set; }


        
    }
  
}
