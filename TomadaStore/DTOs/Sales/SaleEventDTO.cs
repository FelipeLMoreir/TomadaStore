using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TomadaStore.Models.DTOs.Sale;

namespace TomadaStore.Models.DTOs.Sales
{
    public class SaleEventDTO  
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SaleId { get; set; }
        [BsonElement("customerId")]
        public int CustomerId { get; set; }
        [BsonElement("customerName")]
        public string CustomerName { get; set; }
        [BsonElement("products")] 
        public List<SaleProductItemDTO> Products { get; set; }
        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; }
    }
}
