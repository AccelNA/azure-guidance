using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AzureGuidance.Domain;
using AzureGuidance.Common;
using System.Configuration;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        AzureDocumentDBHelper azureDocDBHelper;
        public ProductsController()
        {
            string endpointUrl = ConfigurationManager.AppSettings["Microsoft.DocumentDB.StorageUrl"];
            string authorizationKey = ConfigurationManager.AppSettings["Microsoft.DocumentDB.AuthorizationKey"];
            azureDocDBHelper = new AzureDocumentDBHelper(endpointUrl, authorizationKey);
        }
        public async Task<HttpResponseMessage> PostAddNewProduct(Product product)
        {
            if (null == product)
            {
                throw new ArgumentNullException();
            }
            await azureDocDBHelper.AddDocument(product, "Products");
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, product);
            return response;
        }
        public async Task<ProductResource> Get()
        {
            List<Product> listProductDetails;
            listProductDetails = await azureDocDBHelper.GetProducts();
            ProductResource productResource = new ProductResource();
            productResource.ProductsAvailable = listProductDetails;
            return productResource;
        }
    }
}
