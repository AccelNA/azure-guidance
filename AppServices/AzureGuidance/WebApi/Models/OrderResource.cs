using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureGuidance.Domain;

namespace WebApi.Models
{
    public class OrderResource
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<ProductDetails> OrderProducts { get; set; }

    }
    public class ProductResource
    {

        public IEnumerable<Product> ProductsAvailable { get; set; }
    }
}