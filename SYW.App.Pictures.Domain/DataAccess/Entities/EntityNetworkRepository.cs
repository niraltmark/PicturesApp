using System.Linq;
using Mongo;
using MongoDB.Bson;
using SYW.App.Pictures.Domain.Entities;

namespace SYW.App.Pictures.Domain.DataAccess.Entities
{
	public interface IEntityNetworkRepository
	{
		void UpdateNetwork(ObjectId entityId, Entity[] friends);
		EntityNetwork GetNetwork(ObjectId entityId);
	}

	public class EntityNetworkRepository : IEntityNetworkRepository
	{
		private readonly IMongoStorage<EntityNetwork> _storage;

		public EntityNetworkRepository(IMongoStorage<EntityNetwork> storage)
		{
			_storage = storage;
		}

		public void UpdateNetwork(ObjectId entityId, Entity[] friends)
		{
			var network = _storage.Query().Where(x => x.EntityId == entityId).FirstOrDefault();

			if (network == null)
			{
				_storage.Save(new EntityNetwork {Id = ObjectId.GenerateNewId(), EntityId = entityId, Friends = friends});
			}
			else
			{
				network.Friends = friends;

				_storage.Update(network);
			}
		}

		public EntityNetwork GetNetwork(ObjectId entityId)
		{
			var network = _storage.Query().Where(x => x.EntityId == entityId).FirstOrDefault();

			if (network == null)
				return null;

			return network;
		}
	}
}