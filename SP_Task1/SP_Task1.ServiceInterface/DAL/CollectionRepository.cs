using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SP_Task1.ServiceInterface
{
    public class CollectionRepository<T> : ICollectionRepository<T> where T : ModelBase
    {
        private MongoDbContext mongoDbContext;
        private IMongoCollection<T> mongoCollection;
        private ReplaceOptions upsertOption = new ReplaceOptions { IsUpsert = true };

        public CollectionRepository(MongoDbContext context)
        {
            mongoDbContext = context;
            mongoCollection = mongoDbContext.GetCollection<T>(typeof(T).GetCustomAttribute<TableAttribute>(false).Name);
        }

        public T Find(Guid id)
        {
            return mongoCollection.Find<T>(d => d.Id == id).FirstOrDefault<T>();
        }

        public T Find(IClientSessionHandle session, Guid id)
        {
            return mongoCollection.Find<T>(session, d => d.Id == id).FirstOrDefault<T>();
        }

        public IEnumerable<T> List()
        {
            return mongoCollection.Find<T>(Builders<T>.Filter.Empty).ToList<T>();
        }

        public IEnumerable<T> List(IClientSessionHandle session)
        {
            return mongoCollection.Find<T>(session, Builders<T>.Filter.Empty).ToList<T>();
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> filter)
        {
            return mongoCollection.Find<T>(filter).ToList<T>();
        }

        public IEnumerable<T> List(IClientSessionHandle session, Expression<Func<T, bool>> filter)
        {
            return mongoCollection.Find<T>(session, filter).ToList<T>();
        }

        public void Create(T document)
        {
            CreateInternal(null, document);
        }

        public void Create(IClientSessionHandle session, T document)
        {
            CreateInternal(session, document);
        }

        public void CreateInternal(IClientSessionHandle session, T document)
        {
            document.Id = Guid.NewGuid();
            if (session == null)
                mongoCollection.InsertOne(document);
            else
                mongoCollection.InsertOne(session, document);
        }

        public ReplaceOneResult Upsert(T document)
        {
            return mongoCollection.ReplaceOne<T>(d => d.Id == document.Id, document, upsertOption);
        }

        public ReplaceOneResult Upsert(IClientSessionHandle session, T document)
        {
            return mongoCollection.ReplaceOne<T>(session, d => d.Id == document.Id, document, upsertOption);
        }

        public ReplaceOneResult Update(T document)
        {
            return mongoCollection.ReplaceOne<T>(d => d.Id == document.Id, document);
        }

        public ReplaceOneResult Update(IClientSessionHandle session, T document)
        {
            return mongoCollection.ReplaceOne<T>(session, d => d.Id == document.Id, document);
        }

        public DeleteResult Delete(Guid id)
        {
            return mongoCollection.DeleteOne<T>(d => d.Id == id);
        }

        public DeleteResult Delete(IClientSessionHandle session, Guid id)
        {
            return mongoCollection.DeleteOne<T>(session, d => d.Id == id);
        }
    }
}
