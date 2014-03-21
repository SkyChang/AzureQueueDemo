using AzureQueueDemo.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AzureQueueDemo.Repository.Data
{
    public class DBContextForRead : DbContext
    {
        public DBContextForRead()
            : base("name=DBContextForRead")
        {

        }

        public DbSet<Order> Orders { get; set; }
    }

}