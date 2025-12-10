using MongoDB.Bson.Serialization.Attributes;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleProductItemDTO
    {
        [BsonElement("productId")]
        public string ProductId { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
    }
}