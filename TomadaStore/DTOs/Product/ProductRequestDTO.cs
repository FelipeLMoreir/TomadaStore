using MongoDB.Bson.Serialization.Attributes;
using TomadaStore.Models.DTOs.Category;
using TomadaStore.Models.Models;

namespace TomadaStore.Models.DTOs.Product
{
    public class ProductRequestDTO
    {
        [BsonElement("name")]
        public string Name { get; init; }
        [BsonElement("description")]
        public string Description { get; init; }
        [BsonElement("price")]
        public decimal Price { get; init; }
        [BsonElement("category")]
        public CategoryRequestDTO Category { get; init; }
    }
}
