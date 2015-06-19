using AzureGuidance.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;



namespace AureGuidance.WebSite
{
    public class RESTHelper
    {

        public bool AddProduct(Product product)
        {
            bool productAdded = false;
            var prod = new Product();
            prod.ProductId = product.ProductId;
            prod.ProductName = product.ProductName;
            prod.UnitPrice = product.UnitPrice;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://microsoft-apiappc2c97ea690dc4a59ab2bc747063c4387.azurewebsites.net/");
                var url = "api/Products";
                var response = httpClient.PostAsJsonAsync(url, prod).Result;
                if (response.IsSuccessStatusCode)
                {
                    productAdded = true;
                }
            }
            return productAdded;
        }
        public  async Task<List<Product>> GetAllProducts()
        {
            List<Product> lstProducts = new List<Product>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://microsoft-apiappc2c97ea690dc4a59ab2bc747063c4387.azurewebsites.net/");
                    var url = "api/Products";
                    var response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        dynamic productResource = JsonConvert.DeserializeObject<dynamic>(jsonString);
                        if (null != productResource.ProductsAvailable)
                        {
                            lstProducts = (List<Product>)productResource.ProductsAvailable.ToObject(typeof(List<Product>));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return lstProducts;

        }
        public async Task<List<ProductDetails>> GetProductsOfOrder(Guid id)
        {
            List<ProductDetails> lstProducts = new List<ProductDetails>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://microsoft-apiappc2c97ea690dc4a59ab2bc747063c4387.azurewebsites.net/");
                   
                    var response = await httpClient.GetAsync(string.Format("api/Orders/{0}", id));
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        dynamic orderResource = JsonConvert.DeserializeObject<dynamic>(jsonString);
                        if (null != orderResource.OrderProducts)
                        {
                            lstProducts = (List<ProductDetails>)orderResource.OrderProducts.ToObject(typeof(List<ProductDetails>));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return lstProducts;

        }
        public async Task<List<Order>> GetAllOrders()
        {
            List<Order> lstOrders = new List<Order>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://microsoft-apiappc2c97ea690dc4a59ab2bc747063c4387.azurewebsites.net/");
                var url = "api/Orders";
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    dynamic orderResource = JsonConvert.DeserializeObject<dynamic>(jsonString);
                    if (null != orderResource.Orders)
                    {
                        lstOrders = (List<Order>)orderResource.Orders.ToObject(typeof(List<Order>));
                    }
                }
            }
            return lstOrders;

        }
    }
}