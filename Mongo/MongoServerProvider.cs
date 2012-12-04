using System.Configuration;
using MongoDB.Driver;

namespace Mongo
{
	public interface IMongoServerProvider
	{
		MongoServer Server { get; }
		MongoConnectionStringBuilder Connection { get; }
	}

	public class MongoServerProvider : IMongoServerProvider
	{
		public MongoServerProvider()
		{
			Server = MongoServer.Create(Connection);
		}

		public MongoServer Server { get; private set; }
		public MongoConnectionStringBuilder Connection
		{
			get
			{
				return new MongoConnectionStringBuilder(ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString);
			}
		}
	}
}