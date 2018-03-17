using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Infrastructure
{
    /// <summary>
    /// Don't give any attention to this, it is not important at all.
    /// </summary>
    public class Repository<TObject, TKey> : IRepository<TObject, TKey>
        where TObject : class
    {
        private Dictionary<TKey, TObject> storage = new Dictionary<TKey, TObject>();

        public TObject GetByKey(TKey key)
        {
            if (!storage.ContainsKey(key))
            {
                return null;
            }

            return storage[key];
        }

        public IQueryable<TObject> Query()
        {
            return storage.Values.AsQueryable();
        }

        public void Insert(TObject @object, TKey key)
        {
            if (storage.ContainsKey(key))
            {
                throw new ArgumentException($"Key already exists: {key}", nameof(key));
            }

            storage.Add(key, @object);
        }

        public void Update(TObject @object, TKey key)
        {
            if (!storage.ContainsKey(key))
            {
                throw new ArgumentException($"Key does not exist: {key}", nameof(key));
            }

            storage[key] = @object;
        }
    }
}
