using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.ServiceBus.Messaging;
using AzureGuidance.Domain;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Notifications;

namespace AzureGuidance.Notification
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private bool IsStopped;
        public static TopicClient OrdersTopicClient;
        public static SubscriptionClient NotificationSubClient;

        public override void Run()
        {
            BrokeredMessage receivedMessage = null;
            while (!IsStopped)
            {
                try
                {
                    // Receive the message
                    if (null != NotificationSubClient)
                    { 
                    receivedMessage = NotificationSubClient.Receive();

                    if (receivedMessage != null)
                    {
                        Order orderDetails = receivedMessage.GetBody<Order>();
                        Order order = new Order();
                        order.CustomerName = orderDetails.CustomerName;
                        SendNotificationAsync(order);
                        // Remove message from subscription
                        receivedMessage.Complete();
                        receivedMessage = null;
                    }
                }
                   
                }
                catch (MessagingException e)
                {
                    //unlock message in subscription
                    receivedMessage.Abandon();
                    if (!e.IsTransient)
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }

                    Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    // unlock message in subscription
                    receivedMessage.Abandon();
                    Trace.WriteLine(ex.Message);
                }
            }  

        }

        public override bool OnStart()
        {
            IsStopped = false;
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return base.OnStart();
        }


        public override void OnStop()
        {
            Trace.TraceInformation("AzureGuidance.Notification is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();
            IsStopped = true;
            base.OnStop();

            Trace.TraceInformation("AzureGuidance.Notification has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }

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
                    if (!namespaceManager.SubscriptionExists("OrderTopic", "OrderMessagesForNotification"))
                    {
                        namespaceManager.CreateSubscription("OrderTopic", "OrderMessagesForNotification");
                    }
                    // Initialize the connection to Service Bus Topics
                    OrdersTopicClient = TopicClient.CreateFromConnectionString(connectionString, "OrderTopic");
                    NotificationSubClient = SubscriptionClient.CreateFromConnectionString(connectionString, "OrderTopic", "OrderMessagesForNotification");

                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
        }
        private static async void SendNotificationAsync(Order varOrder)
        {
            try
            {
                DateTime timeNow = DateTime.Now;             
                string template = string.Format("{0:yyyy} {1} {2} {3} {4}", "{ \"data\" : {\"message\":\"Order: ", varOrder.OrderId, "by", varOrder.CustomerName, "\"}}");               
                NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString("", "azureguidancehub");
                var outcome = await hub.SendGcmNativeNotificationAsync(template/*, "mySampleTag"*/);
               
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }
}
