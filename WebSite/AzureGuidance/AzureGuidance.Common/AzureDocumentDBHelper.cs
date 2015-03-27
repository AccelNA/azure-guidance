using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureGuidance.Domain;


namespace AzureGuidance.Common
{
    public  class AzureDocumentDBHelper
    {
        private  DocumentClient client;
        private  Database database;
        private  DocumentCollection collection;
        private  readonly string databaseId = "AzureGuidanceOrderProcessingDB";

        private string endpointUrl;
        private string authorizationKey;

        public AzureDocumentDBHelper(string endPoint,string key)
        {
            endpointUrl = endPoint;
            authorizationKey = key;

        }
        public async Task InitializeDocumentDB()
        {
            if (null == client)
            {
                client = new DocumentClient(new Uri(endpointUrl), authorizationKey);
            }

            //Create Database
            database = client.CreateDatabaseQuery().Where(db => db.Id == databaseId).ToArray().FirstOrDefault();
            if (database == null)
            {
                database = await client.CreateDatabaseAsync(new Database { Id = databaseId });
            }

        }
        private async Task CreateCollection(string collectionId)
        {
            collection = client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == collectionId).ToArray().FirstOrDefault();
            if (null == collection)
            {
                collection = await client.CreateDocumentCollectionAsync(database.SelfLink, new DocumentCollection { Id = collectionId });
            }
        }
        public  async Task AddDocument(object document,string collectionId)
        {
            if (null == client)
            {
               await InitializeDocumentDB();
            }
            await CreateCollection(collectionId);
            await client.CreateDocumentAsync(collection.SelfLink, document);
        }
        public  async Task<List<Order>> GetOrders()
        {
            List<Order> orderDetailsList = new List<Order>();
            if (null == client)
            {
                await InitializeDocumentDB();
            }
            collection = client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == "OrderDetails").ToArray().FirstOrDefault();
            //var query = client.CreateDocumentQuery(collection.SelfLink, "SELECT * FROM OrderDetails");
            var query = from ordr in client.CreateDocumentQuery<Order>(collection.SelfLink) select ordr;
            foreach (Order order in query.AsEnumerable())
            {
                orderDetailsList.Add(order);
            }
            return orderDetailsList;
        }
        public  async Task<List<Product>> GetProducts()
        {
            List<Product> listProducts = new List<Product>(); 
            if (null == client)
            {
                await InitializeDocumentDB();
            }
            collection = client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == "Products").ToArray().FirstOrDefault(); 

            //var query = client.CreateDocumentQuery(collection.SelfLink, "SELECT * FROM Products");
            var query = from p in client.CreateDocumentQuery<Product>(collection.SelfLink) select p;
            foreach (Product prod in query.AsEnumerable())
            {
                listProducts.Add(prod);
            }
            return listProducts;
        }
        public  async Task<List<ProductDetails>> GetOrderProductsDetails(Guid orderID)
        {
            List<ProductDetails> listProductDetails = new List<ProductDetails>();
            if (null == client)
            {
                await InitializeDocumentDB();
            }
            collection = client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == "OrderDetails").ToArray().FirstOrDefault(); 
            //string query = string.Format(@"select c.ProductOrderDetailsList from  orderdetails as c  where c.OrderId = ""{0}""", orderID);
            var queryResult = from c in client.CreateDocumentQuery<Order>(collection.SelfLink) where c.OrderId == orderID select c.ProductOrderDetailsList;
            
            //var queryResult = client.CreateDocumentQuery(collection.SelfLink, query);
            var str = queryResult.AsEnumerable().FirstOrDefault();
            foreach (var pd in str)
            {
                ProductDetails prodDetails = new ProductDetails();
                prodDetails.ProductName = pd.ProductName;
                prodDetails.ProductQuantity = pd.ProductQuantity;
                listProductDetails.Add(prodDetails);
            }
            return listProductDetails;
       }
    }
}
