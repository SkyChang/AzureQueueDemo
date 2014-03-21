using AzureQueueDemo.Core.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureQueueDemo.UI.Web
{
    public class MessageHub: Hub
    {
        public void Send(Order value)
        {
            //Clients.All.broadcastMessage(message);
            Clients.Client(value.ConnectionID).broadcastMessage(value.Name);
        }
    }
}