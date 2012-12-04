using MongoDB.Bson;
using System.Linq;
using SYW.App.Pictures.Domain.Entities;

namespace SYW.App.Pictures.Domain.DataAccess.Entities
{
	public static class EntitiesRepositoryExtensions
	{
		public static Entity Get(this IEntitiesRepository entitiesRepository, ObjectId id)
		{
			return entitiesRepository.GetEntitiesByIds(new[] { id }).EmptyIfNull().FirstOrDefault();
		}

		public static Entity Get(this IEntitiesRepository entitiesRepository, long originalId)
		{
			return entitiesRepository.GetEntitiesByIds(new[] { originalId }).EmptyIfNull().FirstOrDefault();
		}
	}
}