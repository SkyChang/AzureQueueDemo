
using AzureQueueDemo.Core.Models;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

namespace AzureQueueDemo.Storage.Service
{
    /// <summary>
    /// The Proxy decouples frontend from backend business logics. 
    /// </summary>
    public class SearchTaskPUProxy
    {
        private QueueClient mTaskQueue;
        //private SearchTaskProcessingUnits mProcessingUnit;
        private const string QueueName = "Study4Queue";

        public SearchTaskPUProxy()
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
                namespaceManager.CreateQueue(QueueName);
            mTaskQueue = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            //mProcessingUnit = new SearchTaskProcessingUnits(CloudConfigurationManager.GetSetting("StorageAccount"), "inputncbi", "ncbi");
        }

        public void QueueJob(Order order)
        {
            //if (string.IsNullOrEmpty(order.Id))
            //{
            //    order.Id = Guid.NewGuid().ToString("N");
            //}
            //order.LastTimestamp = DateTime.UtcNow.Ticks;
            //order.State = "QUEUED";
            //order.LastMessage = "Queued";
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer (typeof(Order));
            BrokeredMessage msg = new BrokeredMessage(order, jsonSer);
            mTaskQueue.Send(msg);
            //mProcessingUnit.Create(task);
        }
        
        //public void DeleteJob(string id)
        //{
        //    if (id == "OK" || id == "QUEUED")
        //    {
        //        var tasks = mProcessingUnit.List();
        //        foreach (var task in tasks)
        //        {
        //            try
        //            {
        //                if (task.State == id || (id=="QUEUED" && task.State == ""))
        //                    mProcessingUnit.Delete(task.Id, partitionId: task.Id);
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    else 
        //        mProcessingUnit.Delete(id, partitionId:id);
        //}
        
        //public void RetryJob(string id)
        //{
        //    var task = mProcessingUnit.Read(id, partitionId: id);
        //    if (task != null)
        //    {
        //        task.LastTimestamp = DateTime.UtcNow.Ticks;
        //        mTaskQueue.Send(new BrokeredMessage(task));
        //    }
        //}
        
        //public IEnumerable<Entities.SearchTask> List()
        //{
        //    try
        //    {
        //        return mProcessingUnit.List();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        
        //public List<String> ListInputFiles()
        //{
        //    return mProcessingUnit.ListInputFiles();
        //}
    }
}