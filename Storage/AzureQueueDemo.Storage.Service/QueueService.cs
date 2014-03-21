using AzureQueueDemo.Core.Models;
using AzureQueueDemo.Storage.Interface;
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
    public class QueueService : IQueueSevice
    {
        private QueueClient mTaskQueue;
        private const string QueueName = "Study4Queue";

        public QueueService()
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
                namespaceManager.CreateQueue(QueueName);
            mTaskQueue = QueueClient.CreateFromConnectionString(connectionString, QueueName);
        }

        public void SaveQueue(Order order)
        {
            order.LastTimestamp = DateTime.UtcNow.Ticks;
            order.State = "QUEUED";
            DataContractJsonSerializer jsonSer = 
                new DataContractJsonSerializer (typeof(Order));
            BrokeredMessage msg = new BrokeredMessage(order, jsonSer);
            mTaskQueue.Send(msg);
        }

    }
}