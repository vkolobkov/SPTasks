using MongoDB.Driver;
using NLog;
using System;

namespace SP_Task1.ServiceInterface
{
    public class MongoDbContext
    {
        private MongoClient mongoDbClient;
        private IMongoDatabase database;
        private static ILogger Log = LogManager.GetLogger("FileLogger");

        public MongoDbContext(string connectionString)
        {
            try
            {
                mongoDbClient = new MongoClient(connectionString);
                database = mongoDbClient.GetDatabase(MongoUrl.Create(connectionString).DatabaseName);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Something went wrong while initializing MongoDb client.");
                throw;
            }
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName) where T : ModelBase
        {
            return database.GetCollection<T>(collectionName);
        }

        public IClientSessionHandle StartSession()
        {
            return mongoDbClient.StartSession();
        }
    }
}
