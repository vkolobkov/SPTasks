using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace SP_Task1.ServiceInterface
{
    public interface ICollectionRepository<T> where T : ModelBase
    {
        T Find(Guid id);

        T Find(IClientSessionHandle session, Guid id);

        IEnumerable<T> List();

        IEnumerable<T> List(IClientSessionHandle session);

        IEnumerable<T> List(Expression<Func<T, bool>> filter);

        IEnumerable<T> List(IClientSessionHandle session, Expression<Func<T, bool>> filter);

        void Create(T document);

        void Create(IClientSessionHandle session, T document);

        ReplaceOneResult Upsert(T document);

        ReplaceOneResult Upsert(IClientSessionHandle session, T document);

        ReplaceOneResult Update(T document);

        ReplaceOneResult Update(IClientSessionHandle session, T document);

        DeleteResult Delete(Guid id);

        DeleteResult Delete(IClientSessionHandle session, Guid id);
    }
}
