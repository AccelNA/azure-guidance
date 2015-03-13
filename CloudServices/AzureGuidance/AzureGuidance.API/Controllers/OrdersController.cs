using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AzureGuidance.Domain;
using AzureGuidance.Common;
using System.Threading.Tasks;
using AzureGuidance.API.Models;
using System.Configuration;
namespace AzureGuidance.API.Controllers
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
        // GET: api/OrdersAPI
        public async Task<OrderResource> Get()
        {
            List<Order> listOrderDetails;
            listOrderDetails = await azureDocDBHelper.GetOrders();
            OrderResource orderResource = new OrderResource();
            orderResource.Data = listOrderDetails;
           // var resource=new Orde
            return orderResource;
        }

        //GET: api/Orders/5
        public async Task<ProductResource> Get(Guid id)
        {
            List<ProductDetails> listProductDetails;
            listProductDetails = await azureDocDBHelper.GetOrderProductsDetails(id);
            ProductResource productResource = new ProductResource();
            productResource.Data = listProductDetails;
            return productResource;
        }


    }
    

}
