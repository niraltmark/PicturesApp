using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Mongo
{
	public interface IMongoStorage<T> where T : class
	{
		void Save(T t);
		void Update(T t);
		IList<T> GetAll();
		IQueryable<T> Query();
		MongoCollection<T> GetMongoCollection();
	}

	public class MongoStorage<T> : IMongoStorage<T> where T : class
	{ 
		private readonly IMongoDatabaseProvider _mongoDatabaseProvider;

		public MongoStorage(IMongoDatabaseProvider mongoDatabaseProvider)
		{
			_mongoDatabaseProvider = mongoDatabaseProvider;
		}

		public void Save(T t)
		{
			var collection = GetMongoCollection();

			collection.Save(t);
		}

		public void Update(T t)
		{
			var collection = GetMongoCollection();

			collection.Save(t);
		}

		public IList<T> GetAll()
		{
			return GetMongoCollection().FindAll().ToList();
		}

		public IQueryable<T> Query()
		{
			return GetMongoCollection().AsQueryable();
		}


		public MongoCollection<T> GetMongoCollection()
		{
			return _mongoDatabaseProvider.Create().GetCollection<T>(typeof(T).Name.ToLower());
		}
	}
}