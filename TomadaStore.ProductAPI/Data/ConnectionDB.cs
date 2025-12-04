using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TomadaStore.Models.Models;
using TomadaStore.ProductAPI.Data;

namespace TomadaStore.CustomerAPI.Data
{
    public class ConnectionDB
    {
        private readonly IMongoCollection<Product> mongoCollection;
        public ConnectionDB(IOptions<MongoDBSettings> mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            mongoCollection = database.GetCollection<Product>(mongoDbSettings.Value.CollectionName);
        }
        
        public IMongoCollection<Product> GetMongoCollection()
        {
            return mongoCollection;
        }
    }
}
