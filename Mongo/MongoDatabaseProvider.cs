using MongoDB.Driver;

namespace Mongo
{
	public interface IMongoDatabaseProvider
	{
		MongoDatabase Create();
	}

	public class MongoDatabaseProvider : IMongoDatabaseProvider
	{
		private readonly IMongoServerProvider _serverProvider;

		public MongoDatabaseProvider(IMongoServerProvider serverProvider)
		{
			_serverProvider = serverProvider;
		}

		public MongoDatabase Create()
		{
			var server = _serverProvider.Server;

			return server.GetDatabase(_serverProvider.Connection.DatabaseName);
		}
	}
}