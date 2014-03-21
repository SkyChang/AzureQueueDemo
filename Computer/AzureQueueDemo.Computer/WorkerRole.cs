using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System.Runtime.Serialization.Json;
using Microsoft.AspNet.SignalR.Client;
using AzureQueueDemo.Repository.Data;
using AzureQueueDemo.Core.Models;
using AzureQueueDemo.Repository.Interface;

namespace AzureQueueDemo.Computer
{
    public class WorkerRole : RoleEntryPoint
    {
        private IUnitOfWork _uow;

        //Service Bus Queue
        private const string _QueueName = "Study4Queue";
        private QueueClient _qClient;
        private BrokeredMessage _qMessage;

        //Proxy to SignalR Hub
        HubConnection mHubConnection;
        IHubProxy mHubProxy;

        private ManualResetEvent _completedEvent = new ManualResetEvent(false);

        object synRoot = new object();

        public override void Run()
        {
            _qClient.OnMessage((receivedMessage) =>
            {
                Order order = null;
                try
                {
                    //Save the current message because we may need to extend the lock on it.
                    _qMessage = receivedMessage;
                    DataContractJsonSerializer jsonSer = 
                        new DataContractJsonSerializer(typeof(Order));
                    order = receivedMessage.GetBody<Order>(jsonSer);

                    _uow.Repository<Order>().Insert(order);
                    _uow.Save();

                    lock (synRoot)
                    {
                        //Reinitialize SignalR Hub proxy.
                        if (mHubConnection == null)
                            initializeSignalRClient();  
                    }
                    try
                    {
                        mHubProxy.Invoke("Send", new object[] { order });
                    }
                    catch
                    {
                        //Clear the connection so it can be reinitialized next time.
                        lock (synRoot)
                         {
                            try
                            {
                                if (mHubConnection != null)
                                    mHubConnection.Stop();
                            }
                            catch
                            {
                                //Failed to stop. We'll reset next
                            }
                            mHubConnection = null;
                        }
                    }

                    receivedMessage.Complete();
                }
                catch (Exception ex)
                {
                    //Here we mark the message as Complete() instead of Abadon() because we don't want to automatically retry
                    //a failed task. User needs to re-submit the job.
                    System.Diagnostics.Trace.WriteLine(
                        string.Format("Exception during processing the data. {0}, Exception:{1}",
                        receivedMessage.MessageId,
                        ex.ToString()));
                    _qMessage.Complete();
                }
            }, new OnMessageOptions { MaxConcurrentCalls = 1, AutoComplete = false });

            _completedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            _uow = new UnitOfWork();

            //Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(_QueueName))
            {
                namespaceManager.CreateQueue(_QueueName);
            }

            //Initialize the connection to Service Bus Queue
            _qClient = QueueClient.CreateFromConnectionString(connectionString, _QueueName);

            return base.OnStart();
        }

        private void initializeSignalRClient()
        {
            //mHubConnection = new HubConnection(CloudConfigurationManager.GetSetting("HubAddress"));
            mHubConnection = new HubConnection("http://127.0.0.1/signalr", useDefaultUrl: false);
            
            mHubProxy = mHubConnection.CreateHubProxy("MessageHub");
            mHubConnection.Start().Wait();
        }

        public override void OnStop()
        {
            _qClient.Close();
            _completedEvent.Set();
            base.OnStop();
        }
    }
}
