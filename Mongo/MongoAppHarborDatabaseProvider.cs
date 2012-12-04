using System.Configuration;
using MongoDB.Driver;

namespace Mongo
{
	public class MongoAppHarborDatabaseProvider : IMongoDatabaseProvider
	{
		public MongoDatabase Create()
		{
			return MongoDatabase.Create(ConnectionString);
		}

		public static string ConnectionString
		{
			get { return ConfigurationManager.AppSettings.Get("MONGOLAB_URI"); }
		}

		public static bool IsViaAppHarbor
		{
			get { return !string.IsNullOrEmpty(ConnectionString); }
		}
	}
}