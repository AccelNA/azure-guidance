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
        private  DocumentClient _client;
        private  Database database;
        private  DocumentCollection collection;
        private  readonly string databaseId = "AzureGuidanceOrderProcessingDB";

        private string endpointUrl;
        private string authorizationKey;

        public AzureDocumentDBHelper(string endPoint,string authorizationKey)
        {
            endpointUrl = endPoint;
            authorizationKey = authorizationKey;

        }
        public async Task InitializeDocumentDB()
        {
            if (null == _client)
            {
                _client = new DocumentClient(new Uri(endpointUrl), authorizationKey);
            }

            //Create Database
            database = _client.CreateDatabaseQuery().Where(db => db.Id == databaseId).ToArray().FirstOrDefault();
            if (database == null)
            {
                database = await _client.CreateDatabaseAsync(new Database { Id = databaseId });
            }

        }
        private async Task CreateCollection(string collectionId)
        {
            collection = _client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == collectionId).ToArray().FirstOrDefault();
            if (null == collection)
            {
                collection = await _client.CreateDocumentCollectionAsync(database.SelfLink, new DocumentCollection { Id = collectionId });
            }
        }
        public  async Task AddDocument(object document,string collectionId)
        {
            if (null == _client)
            {
               await InitializeDocumentDB();
            }
            await CreateCollection(collectionId);
            await _client.CreateDocumentAsync(collection.SelfLink, document);
        }
        public  async Task<List<Order>> GetOrders()
        {
            List<Order> orderDetailsList = new List<Order>();
            if (null == _client)
            {
                await InitializeDocumentDB();
            }
            await CreateCollection("OrderDetails");
            var query =  _client.CreateDocumentQuery(collection.SelfLink, "SELECT * FROM OrderDetails");
            foreach (Order order in query.AsEnumerable())
            {
                orderDetailsList.Add(order);
            }
            return orderDetailsList;
        }
        public  async Task<List<Product>> GetProducts()
        {
            List<Product> listProducts = new List<Product>(); 
            if (null == _client)
            {
                await InitializeDocumentDB();
            }
            await CreateCollection("Products");
            var query = _client.CreateDocumentQuery(collection.SelfLink, "SELECT * FROM Products");
            foreach (Product prod in query.AsEnumerable())
            {
                listProducts.Add(prod);
            }
            return listProducts;
        }
        public  async Task<List<ProductDetails>> GetOrderProductsDetails(Guid orderID)
        {
            List<ProductDetails> listProductDetails = new List<ProductDetails>();
            if (null == _client)
            {
                await InitializeDocumentDB();
            }
            await CreateCollection("OrderDetails");
            //var query = client.CreateDocumentQuery(collection.SelfLink, "SELECT * from OrderDetails where OrderId ="+ "\""+orderID+"\"");

            string sQuery = string.Format(@"select c.ProductOrderDetailsList from  orderdetails as c  where c.OrderId = ""{0}""", orderID);
            //9858af52-70d8-465c-8b14-046b8148c98b
            var queryResult = _client.CreateDocumentQuery(collection.SelfLink, sQuery);
            //var family = query.AsEnumerable().FirstOrDefault();
            var str = queryResult.AsEnumerable().FirstOrDefault();
            foreach (var pd in str.ProductOrderDetailsList)
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
