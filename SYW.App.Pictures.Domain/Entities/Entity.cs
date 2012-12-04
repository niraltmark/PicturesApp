using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SYW.App.Pictures.Domain.Entities
{
	public class Entity
	{
		[BsonId]
		public ObjectId Id { get; set; }

		public long OriginalId { get; set; }

		public string Name { get; set; }

		public string ImageUrl { get; set; }

		public string OriginalEmail { get; set; }

		public string Email { get; set; }

        public DateTime LastActivity { get; set; }

		public string PlatformOfflineToken { get; set; }

		public BsonDateTime MemberSince { get; set; }
	}
}