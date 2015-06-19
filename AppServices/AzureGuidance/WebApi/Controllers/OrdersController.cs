using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using AzureGuidance.Domain;
using AzureGuidance.Common;
using WebApi.Models;
using System.Configuration;


namespace WebApi.Controllers
{
    public class OrdersController : ApiController
    {
        AzureDocumentDBHelper azureDocDBHelper;
        public OrdersController()
        {
            string endpointUrl = ConfigurationManager.AppSettings["Microsoft.DocumentDB.StorageUrl"];
            string authorizationKey = ConfigurationManager.AppSettings["Microsoft.DocumentDB.AuthorizationKey"];
            azureDocDBHelper = new AzureDocumentDBHelper(endpointUrl, authorizationKey);
        }
        public async Task<OrderResource> GetOrders()
        {
            List<Order> listOrderDetails;
            listOrderDetails = await azureDocDBHelper.GetOrders();
            OrderResource orderResource = new OrderResource();
            orderResource.Orders = listOrderDetails;
            return orderResource;
        }
        public async Task<OrderResource> GetProductsOfAnOrder(Guid id)
        {
            List<ProductDetails> listProductDetails;
            listProductDetails = await azureDocDBHelper.GetOrderProductsDetails(id);
            OrderResource orderResource = new OrderResource();
            orderResource.OrderProducts = listProductDetails;
            return orderResource;
        }
        public async Task<HttpResponseMessage> PostAddNewOrder(Order order)
        {
            if (null == order)
            {
                throw new ArgumentNullException();
            }
            await azureDocDBHelper.AddDocument(order, "OrderDetails");
            var response = Request.CreateResponse<Order>(HttpStatusCode.Created, order);
            return response;
        }
        
       
    }
}
