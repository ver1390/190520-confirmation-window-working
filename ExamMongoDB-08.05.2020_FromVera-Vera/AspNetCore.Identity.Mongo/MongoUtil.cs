using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace AspNetCore.Identity.Mongo
{
    internal static class MongoUtil
    {
        public static IMongoCollection<TItem> FromConnectionString<TItem>(string connectionString, string collectionName)
        {
            var type = typeof(TItem);

            if (connectionString != null)
            {
                var url = new MongoUrl(connectionString);
                var client = new MongoClient(connectionString);
                return client.GetDatabase(url.DatabaseName ?? "default")
                    .GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());
            }

            return new MongoClient().GetDatabase("default")
                .GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());
        }


        public static async Task<TItem> FirstOrDefaultAsync<TItem>(this IMongoCollection<TItem> mongoCollection, Expression<Func<TItem, bool>> p)
        {
            return await (await mongoCollection.FindAsync(p)).FirstOrDefaultAsync();
        }


        public static async Task<IEnumerable<TItem>> WhereAsync<TItem>(this IMongoCollection<TItem> mongoCollection, Expression<Func<TItem, bool>> p)
        {
            return (await mongoCollection.FindAsync(p)).ToEnumerable();
        }

        public static async Task<List<TItem>> All<TItem>(this IMongoCollection<TItem> mongoCollection)
        {
            return (await (await mongoCollection.FindAsync(Builders<TItem>.Filter.Empty)).ToListAsync());
        }

    }
}
