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
using Microsoft.ServiceBus.Notifications;

namespace AzureGuidance.Notification
{
    public class Functions
    {

        public static async Task ProcessTopicMessage([ServiceBusTrigger("OrderTopic", "OrderMessagesForNotification")]  BrokeredMessage message, TextWriter logger)
        {
            logger.WriteLine("Topic message: " + message);
            if (message != null)
            {
                Order orderDetails = message.GetBody<Order>();
                Order order = new Order();
                order.CustomerName = orderDetails.CustomerName;
                order.OrderId = orderDetails.OrderId;
                await SendNotificationAsync(order, logger);
                // Remove message from subscription
                message.Complete();
                message = null;
            }
        }
        private static async Task SendNotificationAsync(Order varOrder, TextWriter logger)
        {
            try
            {
                DateTime timeNow = DateTime.Now;
                string template = string.Format("{0} {1} {2} {3} {4}", "{ \"data\" : {\"message\":\"Order: ", varOrder.OrderId, "by", varOrder.CustomerName, "\"}}");

                string endpointUrl = "Endpoint=sb://azureguidancenotificationns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=i9u54hy58z80jyUO5oNAnfQ7rJhuwhilij7je5pHCuw=";
                NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(endpointUrl, "AzureGuidanceNotification");
                var outcome = await hub.SendGcmNativeNotificationAsync(template);

            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
