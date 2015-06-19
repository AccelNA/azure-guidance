using AzureGuidance.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AzureGuidance.WebJob
{
    class RESTHelper
    {
        public static bool AddOrder(Order order)
        {
            bool orderAdded = false;
            var ordr = new Order();
            ordr.OrderId = order.OrderId;
            ordr.ProductOrderDetailsList = order.ProductOrderDetailsList;
            ordr.TotalDue = order.TotalDue;
            ordr.CustomerName = order.CustomerName;
            ordr.EmailId = order.EmailId;
            ordr.orderStatus = order.orderStatus;
            ordr.OrderDate = order.OrderDate;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://microsoft-apiappc2c97ea690dc4a59ab2bc747063c4387.azurewebsites.net/");
                var url = "api/Orders";
                var response = httpClient.PostAsJsonAsync(url, ordr).Result;
                if (response.IsSuccessStatusCode)
                {
                    orderAdded = true;
                }
            }
            return orderAdded;
        }
    }
}
