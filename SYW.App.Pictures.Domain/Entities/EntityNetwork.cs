using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SYW.App.Pictures.Domain.Entities
{
	public class EntityNetwork
	{
		[BsonId]
		public ObjectId Id { get; set; }

		public ObjectId EntityId { get; set; }

		public Entity[] Friends { get; set; }
	}
}