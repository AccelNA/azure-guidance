using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureGuidance.Domain;

namespace AzureGuidance.API.Models
{
    public class OrderResource
    {
        public IEnumerable<Order> Data { get; set; }
    }
    public class ProductResource
    {
        public IEnumerable<ProductDetails> Data { get; set; }
    }
}