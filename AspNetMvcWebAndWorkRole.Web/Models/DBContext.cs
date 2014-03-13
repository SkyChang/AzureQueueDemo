using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AspNetMvcWebAndWorkRole.Web.Models
{
    public class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {

        }

        public DbSet<Order> Orders { get; set; }
    }

}