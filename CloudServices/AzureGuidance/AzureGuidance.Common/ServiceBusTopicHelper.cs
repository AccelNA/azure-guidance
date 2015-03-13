using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;


namespace AzureGuidance.Common
{
    public static class ServiceBusTopicHelper
    {
        public static TopicClient OrdersTopicClient;
        public static SubscriptionClient SubClient, SubClient1;


        public static void Initialize()
        {
            if (null == OrdersTopicClient)
            {
                try
                {
                    string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBusTopics.ConnectionString");
                    // string connectionString = RoleEnvironment.GetConfigurationSettingValue("Microsoft.ServiceBusTopics.ConnectionString");
                    var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
                    if (!namespaceManager.TopicExists("OrderTopic"))
                    {
                        namespaceManager.CreateTopic("OrderTopic");
                    }
                    if (!namespaceManager.SubscriptionExists("OrderTopic", "OrderMessages"))
                    {
                        namespaceManager.CreateSubscription("OrderTopic", "OrderMessages");
                    }
                    // Initialize the connection to Service Bus Topics
                    OrdersTopicClient = TopicClient.CreateFromConnectionString(connectionString, "OrderTopic");
                    SubClient = SubscriptionClient.CreateFromConnectionString(connectionString, "OrderTopic", "OrderMessages");
                   
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
        }
    }
}
