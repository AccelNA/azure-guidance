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
using System.Configuration;

namespace AzureGuidance.WebJob
{
    public class AzureDocumentDBHelper
    {
        private static DocumentClient client;
        private static Database database;
        private static DocumentCollection collection;
        private static readonly string databaseId = "AzureGuidanceOrderProcessingDB";

              
        public static async Task InitializeDocumentDB()
        {
            string endpointUrl = "https://azureguidancedocdb.documents.azure.com:443/";
            string authorizationKey = "itXOcOUMvV5Tid0yl1rHBzH/ejvKtdnxY3bAj/W+xdUVVM4pkNiJbxEsUKRn3aDuJJMrTtscpT5PlKDSivmeoA==";
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
        private static async Task CreateCollection(string collectionId)
        {
            collection = client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == collectionId).ToArray().FirstOrDefault();
            if (null == collection)
            {
                collection = await client.CreateDocumentCollectionAsync(database.SelfLink, new DocumentCollection { Id = collectionId });
            }
        }
        public static async Task AddDocument(object document, string collectionId)
        {
            if (null == client)
            {
                await InitializeDocumentDB();
            }
            await CreateCollection(collectionId);
            await client.CreateDocumentAsync(collection.SelfLink, document);
        }
    }
}
