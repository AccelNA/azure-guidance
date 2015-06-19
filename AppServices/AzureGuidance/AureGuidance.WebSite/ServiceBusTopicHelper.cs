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


namespace AzureGuidance.ServiceBusTopics
{
    public static class ServiceBusTopicHelper
    {
        public static TopicClient OrdersTopicClient;
        public static SubscriptionClient SubClient;


        public static void Initialize()
        {
            if (null == OrdersTopicClient)
            {
                try
                {
                    string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBusTopics.ConnectionString"];
                    // string connectionString = RoleEnvironment.GetConfigurationSettingValue("Microsoft.ServiceBusTopics.ConnectionString");
                    var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
                    if (!namespaceManager.TopicExists("TestOrderTopic"))
                    {
                        namespaceManager.CreateTopic("TestOrderTopic");
                    }
                    if (!namespaceManager.SubscriptionExists("TestOrderTopic", "OrderMessages"))
                    {
                        namespaceManager.CreateSubscription("TestOrderTopic", "OrderMessages");
                    }
                    // Initialize the connection to Service Bus Topics
                    OrdersTopicClient = TopicClient.CreateFromConnectionString(connectionString, "TestOrderTopic");
                    SubClient = SubscriptionClient.CreateFromConnectionString(connectionString, "TestOrderTopic", "OrderMessages");
                    //subclient1 = SubscriptionClient.CreateFromConnectionString(connectionString,"TestOrderTopic","OrderMessages1");
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }
            }
        }
    }
}
