using System;
using System.Linq;

namespace OrderService.Infrastructure
{
    public interface IRepository<TObject, TKey>
        where TObject : class
    {
        TObject GetByKey(TKey key);

        IQueryable<TObject> Query();

        void Update(TObject @object, TKey key);

        void Insert(TObject @object, TKey key);
    }
}
