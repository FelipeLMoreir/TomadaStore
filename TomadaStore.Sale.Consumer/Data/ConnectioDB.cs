using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TomadaStore.Models.Models;

namespace TomadaStore.Sale.Consumer.Data
{
    public class ConnectionDB
    {
        public readonly IMongoCollection<Sale> mongoCollection;

        public ConnectionDB(IOptions<MongoDbSettings> mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            mongoCollection = database.GetCollection<Sale>(mongoDbSettings.Value.CollectionName);
        }

        public IMongoCollection<Sale> GetMongoCollection()
        {
            return mongoCollection;
        }
        public void InserOneAsync()
        {

        }
    }
}