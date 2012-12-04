using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace SYW.App.Messages.Web.Services
{
	public class EntityContext
	{
		// To similar to entity, why not use it?

		public string ObjectId { get; set; }
		public long OriginalId { get; set; }
		public string Name { get; set; }
		public string ImageUrl { get; set; }
		public EntityType EntityType { get; set; }
		public string Token { get; set; }
		public DateTime? MemberSince { get; set; }

		[JsonIgnore]
		public ObjectId Id
		{
			get { return MongoDB.Bson.ObjectId.Parse(ObjectId); }
		}
	}
}