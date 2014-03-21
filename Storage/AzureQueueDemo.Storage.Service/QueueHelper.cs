using AzureQueueDemo.Core.Models;
using AzureQueueDemo.Repository.Interface;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureQueueDemo.Storage.Service
{
    public class QueueHelper
    {
        private CloudQueueClient _queueClient;
        private IUnitOfWork _repository;

        private static readonly string queueName = "skyorder";

        public QueueHelper(IUnitOfWork repository)
        {
            _repository = repository;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnection"));
            //CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            _queueClient = storageAccount.CreateCloudQueueClient();
        }

        // Puts a serialized fixit onto the queue.
        public async Task SendMessageAsync(Order order)
        {
            CloudQueue queue = _queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();
            queue.CreateIfNotExists();
            var orderJson = JsonConvert.SerializeObject(order);
            CloudQueueMessage message = new CloudQueueMessage(orderJson);

            await queue.AddMessageAsync(message);
        }

        // Processes any messages on the queue.
        public async Task ProcessMessagesAsync()
        {
            CloudQueue queue = _queueClient.GetQueueReference(queueName);
            await queue.CreateIfNotExistsAsync();

            while (true)
            {
                CloudQueueMessage message = await queue.GetMessageAsync();
                if (message == null)
                {
                    break;
                }
                Order order = JsonConvert.DeserializeObject<Order>(message.AsString);
                _repository.Repository<Order>().Insert(order);
                _repository.Save();
                await queue.DeleteMessageAsync(message);
            }
        }
    }
    
}