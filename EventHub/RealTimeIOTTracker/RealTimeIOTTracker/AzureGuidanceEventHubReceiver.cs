using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using RealTimeWebApp;
using Newtonsoft.Json;

namespace AzureGuidanceEventHub
{
    
    public class AzureGuidanceEventHubReceiver : Hub
    {
        string eventHubName;
        string connectionString;
        EventHubConsumerGroup defaultConsumerGroup;
        EventHubClient eventHubClient;
        EventProcessorHost eventProcessorHost;
        public async void ProcessEvents()
        {
            eventHubName = "azureguidanceevnthub";
            connectionString = GetServiceBusConnectionString();
            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            defaultConsumerGroup = eventHubClient.GetDefaultConsumerGroup();
            string blobConnectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"]; // Required for checkpoint/state
            eventProcessorHost = new EventProcessorHost("AzureGuidanceReceiver", eventHubClient.Path, defaultConsumerGroup.GroupName, connectionString, blobConnectionString);
            await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();
                        
        }
       
        private string GetServiceBusConnectionString()
        {
                string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    return string.Empty;
                }
                ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(connectionString);
                builder.TransportType = TransportType.Amqp;
                return builder.ToString();
            
           
        }
    }
}