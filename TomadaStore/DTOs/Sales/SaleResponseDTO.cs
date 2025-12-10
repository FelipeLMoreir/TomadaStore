using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleResponseDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("customer")]
        public CustomerResponseDTO Customer { get; set; }
        [BsonElement("products")]
        public List<ProductResponseDTO> Products { get; set; } = new();
        [BsonElement("saleDate")]
        public DateTime SaleDate { get; set; }
        [BsonElement("totalPrice")]
        public decimal TotalPrice { get; set; }
    }
}
