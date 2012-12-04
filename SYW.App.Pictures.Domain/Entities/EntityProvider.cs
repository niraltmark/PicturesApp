using System.Linq;
using CommonGround.Utilities;
using MongoDB.Bson;
using SYW.App.Pictures.Domain.DataAccess.Entities;
using SYW.App.Pictures.Domain.Services.Platform;

namespace SYW.App.Pictures.Domain.Entities
{
	public interface IEntityProvider
	{
		Entity GetEntityFromPlatform();
	}

	public class EntityProvider : IEntityProvider
	{
		private readonly IEntitiesRepository _entitiesRepository;
		private readonly IUserService _userService;

		public EntityProvider(IEntitiesRepository entitiesRepository, IUserService userService)
		{
			_entitiesRepository = entitiesRepository;
			_userService = userService;
		}

		public Entity GetEntityFromPlatform()
		{
			var originalEntity = _userService.CurrentUser();
			if (originalEntity == null)
				return null;

			var entity = _entitiesRepository.GetEntitiesByIds(new[] { originalEntity.id }).FirstOrDefault();

			if (entity != null)
			{
				entity.ImageUrl = originalEntity.imageUrl;
				entity.Name = originalEntity.name;
				entity.OriginalEmail = entity.OriginalEmail;
				entity.LastActivity = SystemTime.Now();

				_entitiesRepository.Update(entity);

				return entity;
			}

			entity = new Entity
			         	{
			         		Id = ObjectId.GenerateNewId(),
			         		OriginalId = originalEntity.id,
			         		Name = originalEntity.name,
			         		ImageUrl = originalEntity.imageUrl,
			         		OriginalEmail = originalEntity.email,
			         		LastActivity = SystemTime.Now(),
			         		MemberSince = SystemTime.Now()
			         	};

			_entitiesRepository.Add(entity);

			return entity;
		}
	}
}