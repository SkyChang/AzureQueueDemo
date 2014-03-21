using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AzureQueueDemo.Core.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long LastTimestamp { get; set; }

        public string State { get; set; }
        
        public string LastMessage { get; set; }

        public string ConnectionID { get; set; }
    }
}