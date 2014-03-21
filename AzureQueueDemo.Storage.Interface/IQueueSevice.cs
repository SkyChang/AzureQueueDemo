using AzureQueueDemo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureQueueDemo.Storage.Interface
{
    public interface IQueueSevice
    {
        void SaveQueue(Order order);
    }
}
