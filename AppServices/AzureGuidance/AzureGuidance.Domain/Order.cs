using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureGuidance.Domain
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public decimal TotalDue { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public string EmailId { get; set; }
        public List<ProductDetails> ProductOrderDetailsList { get; set; }
        public DateTime OrderDate { get; set; }
        [DefaultValue("Processed")]
        public string orderStatus { get; set; }
    }
   
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public class AddProduct
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        public int ProductQuantity { get; set; }//---
        [DefaultValue(false)]
        public bool SelectProduct { get; set; } //---
    }


    public class ProductDetails
    {
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
    }
    public class ProductOrder
    {
        public Order order { get; set; }
        public List<AddProduct> lstProducts { get; set; }
    }

    //public class ProductOrderQuantity
    //{
    //    public int ProductQuantity { get; set; }//---
    //}
   
}
