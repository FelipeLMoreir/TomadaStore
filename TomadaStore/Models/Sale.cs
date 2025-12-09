using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TomadaStore.Models.Models
{
    public class Sale
    {
        [JsonPropertyName("id")]
        public string Id { get; private set; }
        [JsonPropertyName("customer")]
        public Customer Customer { get; private set; }
        [JsonPropertyName("products")]
        public List<Product> Products { get; private set; }
        [JsonPropertyName("saleDate")]
        public DateTime SaleDate { get; private set; }
        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice { get; private set; }

        public Sale()
        {
            
        }

        public Sale(Customer customer, List<Product> products, decimal totalPrice)
        {
            Customer = customer;
            Products = products;
            SaleDate = DateTime.UtcNow;
            TotalPrice = totalPrice;
        }
    }
}
