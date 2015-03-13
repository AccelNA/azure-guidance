using AzureGuidance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ServiceBus.Messaging;
using AzureGuidance.Domain;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;


namespace AzureGuidance.Web.Controllers
{
   
    public class OrderController : Controller
    {
        AzureDocumentDBHelper azureDocDBHelper;
        public OrderController()
        {
            string endpointUrl = ConfigurationManager.AppSettings["Microsoft.DocumentDB.StorageUrl"];
            string authorizationKey = ConfigurationManager.AppSettings["Microsoft.DocumentDB.AuthorizationKey"];
            azureDocDBHelper = new AzureDocumentDBHelper(endpointUrl, authorizationKey);
        }
        public async Task<ActionResult> DisplayProducts()
        {
            List<Product> lstProducts;
            ProductOrder productOrder = new ProductOrder();
            List<AddProduct> lstAddProducts = new List<AddProduct>();
            lstProducts = await azureDocDBHelper.GetProducts();
            foreach (Product pd in lstProducts)
            {
                AddProduct addPd = new AddProduct();
                addPd.ProductId = pd.ProductId;
                addPd.ProductName = pd.ProductName;
                addPd.UnitPrice = pd.UnitPrice;
                lstAddProducts.Add(addPd);
            }
            productOrder.lstProducts = lstAddProducts;
            return View(productOrder);
        }
        public async Task<ActionResult> DisplayProductsForAdd()
        {
            List<Product> lstProducts;
            lstProducts = await azureDocDBHelper.GetProducts();
            return View("Products", lstProducts);
        }

        [HttpPost]
        public ActionResult SubmitOrder(ProductOrder productOrder)
        {

            //if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.SubmitResponse = string.Empty;
                    Order order = new Order();
                    order.CustomerName = productOrder.order.CustomerName;
                    order.OrderDate = DateTime.Now;
                    order.EmailId = productOrder.order.EmailId;
                    order.ProductOrderDetailsList = new List<ProductDetails>();
                    foreach (AddProduct p in productOrder.lstProducts)
                    {
                        if (true == p.SelectProduct)
                        {
                            ProductDetails productDetails = new ProductDetails();
                            productDetails.ProductName = p.ProductName;
                            productDetails.ProductQuantity = p.ProductQuantity;
                            order.TotalDue += p.UnitPrice * p.ProductQuantity;
                            order.orderStatus = "Processed";
                            order.OrderId  = Guid.NewGuid();
                            order.ProductOrderDetailsList.Add(productDetails);
                            p.ProductQuantity = 0;
                            p.SelectProduct = false;
                        }

                    }
                    var message = new Microsoft.ServiceBus.Messaging.BrokeredMessage(order);
                    ServiceBusTopicHelper.OrdersTopicClient.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            ProductOrder pOrder = new ProductOrder();
            List<AddProduct> lstProducts = new List<AddProduct>();

            pOrder.lstProducts = productOrder.lstProducts;
            ModelState.Clear();
            ViewBag.SubmitResponse = "Order proccessed successfully.";
            return View("DisplayProducts", pOrder);
        }
        private void GetProducts(List<AddProduct> productLst)
        {
            foreach(AddProduct p in productLst)
            {
                //p.ProductQuantity = "";
                p.SelectProduct = false;
            }
        }

        public async Task<ActionResult> DisplayProductsOfOrder(Guid id )
        {
            List<ProductDetails> listProductDetails;
            listProductDetails = await azureDocDBHelper.GetOrderProductsDetails(id);
           return PartialView("_CustomerOrder", listProductDetails);
           
        }
        

        public async Task<ActionResult> ListOrder()
        {
            List<Order> listOrderDetails;
            listOrderDetails = await azureDocDBHelper.GetOrders();
            return View(listOrderDetails);
        }
    
        public async Task<ActionResult> AddProduct(string productName, string unitPrice)
        {
            Product prod = new Product();
            prod.ProductId = Guid.NewGuid().ToString();
            prod.ProductName = productName;
            decimal price;
            decimal.TryParse(unitPrice, out price);
            prod.UnitPrice = price;
            await azureDocDBHelper.AddDocument(prod, "Products");
            return View("Products");
        }
         //private string GetUniqueKey()
         // {
         // int maxSize  = 8 ;
         // char[] chars = new char[62]; 
         // string a;
         // a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
         // chars = a.ToCharArray();
         // int size  = maxSize ;
         // byte[] data = new byte[1];
         // RNGCryptoServiceProvider  crypto = new RNGCryptoServiceProvider();
         // crypto.GetNonZeroBytes(data) ;
         // size =  maxSize ;
         // data = new byte[size];
         // crypto.GetNonZeroBytes(data);
         // StringBuilder result = new StringBuilder(size) ;
         // foreach(byte b in data )
         // { 
         //     result.Append(chars[b % (chars.Length)]);
         // }
         //  return result.ToString();
         // }
        
    }
}