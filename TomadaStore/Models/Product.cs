using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TomadaStore.Models.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public ObjectId Id { get; private set; }
        [JsonPropertyName("name")]
        public string Name { get; private set; }
        [JsonPropertyName("description")]
        public string Description { get; private set; }
        [JsonPropertyName("price")]
        public decimal Price { get; private set; }
        [JsonPropertyName("category")]
        public Category Category { get; private set; }

        public Product()
        {
            
        }

        public Product(string name, string description, decimal price, Category category)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }
        public Product(string id, string name, string description, decimal price, Category category)
        {
            Id = ObjectId.Parse(id);
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }
    }
}
