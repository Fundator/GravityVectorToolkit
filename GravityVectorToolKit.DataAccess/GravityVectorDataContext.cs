using GravityVectorToKML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GravityVectorToolKit.DataAccess
{
    public class GravityVectorDataContext : DbContext
    {
        public DbSet<NormalPointG> NormalPoints { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(@"Database=GravityVectorToolKit;Data Source=localhost;User Id=gvtk;Password=gvtk;");
        }
    }
}
