using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SYW.App.Pictures.Domain.Entities
{
	public class Picture
	{
		[BsonId]
		public ObjectId Id { get; set; }
		
		public long OriginalId { get; set; }

		public string ImageUrl { get; set; }

		public ObjectId EntityId { get; set; }

		public long OriginalProductId { get; set; }

		public int Status { get; set; }

		public DateTime Date { get; set; }
	}

	public enum PictureStatus
	{
		Invalid = 0,
		Valid = 1
	}
}