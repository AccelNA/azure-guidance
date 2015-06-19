using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

using AzureGuidance.Domain;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;


namespace AzureGuidance.WebJob
{
    public class Functions
    {
        //AzureDocumentDBHelper azureDocDBHelper;
       
        // This function will get triggered/executed when a new message is send to the topic

        public static void ProcessTopicMessage([ServiceBusTrigger("OrderTopic", "OrderMessagesForDocumentDB")]  BrokeredMessage message, TextWriter logger)
        {
            logger.WriteLine("Topic message: " + message);
            if (message != null)
            {
                Order orderDetails = message.GetBody<Order>(); 
                Order order = new Order();
                order.CustomerName = orderDetails.CustomerName;
                order.EmailId = orderDetails.EmailId;
                order.ProductOrderDetailsList = orderDetails.ProductOrderDetailsList;
                order.OrderDate = orderDetails.OrderDate;
                order.TotalDue = orderDetails.TotalDue;
                order.OrderId = orderDetails.OrderId;
                order.orderStatus = "Processed";
                 //Remove message from subscription
                message.Complete();
                RESTHelper.AddOrder(order);
                message = null;
            }
        }
    }
}
