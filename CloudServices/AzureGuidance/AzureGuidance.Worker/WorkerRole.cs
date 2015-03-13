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
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using AzureGuidance.Domain;
using AzureGuidance.Common;
using System.Configuration;

namespace AzureGuidance.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private bool IsStopped;
        public static TopicClient OrdersTopicClient;
        public static SubscriptionClient SubClient;
        private AzureDocumentDBHelper azureDocDBHelper;

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole1 is running");
            //BrokeredMessage receivedMessage = null;
            //while (!IsStopped)
            //{
            //    try
            //    {
            //        // Receive the message

            //        receivedMessage = SubClient.Receive();

            //        if (receivedMessage != null)
            //        {
            //            Order orderDetails = receivedMessage.GetBody<Order>();
            //            Order order = new Order();
            //            order.CustomerName = orderDetails.CustomerName;
            //            order.EmailId = orderDetails.EmailId;
            //            order.ProductOrderDetailsList = orderDetails.ProductOrderDetailsList;
            //            order.OrderDate = orderDetails.OrderDate;
            //            order.TotalDue = orderDetails.TotalDue;
            //            order.orderStatus = "Processed";
            //            // Remove message from subscription
            //            receivedMessage.Complete();
            //            order.OrderId = Guid.NewGuid();
            //            azureDocDBHelper.AddDocument(order, "OrderDetails");
            //            receivedMessage = null;
            //        }

            //    }
            //    catch (MessagingException e)
            //    {
            //        if (null != receivedMessage)
            //        {
            //            //unlock message in subscription
            //            receivedMessage.Abandon();
            //        }
            //        if (!e.IsTransient)
            //        {
            //            Trace.WriteLine(e.Message);
            //            throw;
            //        }

            //        Thread.Sleep(10000);
            //    }
            //    catch (Exception ex)
            //    {
            //        // unlock message in subscription
            //        receivedMessage.Abandon();
            //        Trace.WriteLine(ex.Message);
            //    }
            //}
            RunAsync().Wait();
        }
        private async Task RunAsync()
        {

            BrokeredMessage receivedMessage = null;
            while (!IsStopped)
            {
                try
                {
                    // Receive the message

                    receivedMessage = SubClient.Receive();

                    if (receivedMessage != null)
                    {
                        Order orderDetails = receivedMessage.GetBody<Order>();
                        Order order = new Order();
                        order.CustomerName = orderDetails.CustomerName;
                        order.EmailId = orderDetails.EmailId;
                        order.ProductOrderDetailsList = orderDetails.ProductOrderDetailsList;
                        order.OrderDate = orderDetails.OrderDate;
                        order.TotalDue = orderDetails.TotalDue;
                        order.orderStatus = "Processed";
                        // Remove message from subscription
                        receivedMessage.Complete();
                        order.OrderId = Guid.NewGuid();
                        await azureDocDBHelper.AddDocument(order, "OrderDetails");
                        receivedMessage = null;
                    }

                }
                catch (MessagingException e)
                {
                    if (null != receivedMessage)
                    {
                        //unlock message in subscription
                        receivedMessage.Abandon();
                    }
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
                try
                {
                    Initialize();
                    string endpointUrl = ConfigurationManager.AppSettings["Microsoft.DocumentDB.StorageUrl"];
                    string authorizationKey = ConfigurationManager.AppSettings["Microsoft.DocumentDB.AuthorizationKey"];
                    azureDocDBHelper = new AzureDocumentDBHelper(endpointUrl, authorizationKey);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return base.OnStart();
        }

        public override void OnStop()
        {
            IsStopped = true;
            base.OnStop();
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
                    if (!namespaceManager.SubscriptionExists("OrderTopic", "OrderMessagesForDocumentDB"))
                    {
                        namespaceManager.CreateSubscription("OrderTopic", "OrderMessagesForDocumentDB");
                    }
                    // Initialize the connection to Service Bus Topics
                    OrdersTopicClient = TopicClient.CreateFromConnectionString(connectionString, "OrderTopic");
                    SubClient = SubscriptionClient.CreateFromConnectionString(connectionString, "OrderTopic", "OrderMessagesForDocumentDB");

                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
        }
       
    }
}
