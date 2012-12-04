using System.Linq;
using MongoDB.Bson;
using SYW.App.Pictures.Domain.DataAccess.Entities;
using SYW.App.Pictures.Domain.Services.Platform;

namespace SYW.App.Pictures.Domain.Entities
{
	public interface IEntitiesInitializer
	{
		void Init(long[] entitiesIds);
	}

	public class EntitiesInitializer : IEntitiesInitializer
	{
		private readonly IUserService _userService;
		private readonly IEntitiesRepository _entitiesRepository;

		public EntitiesInitializer(IUserService userService, IEntitiesRepository entitiesRepository)
		{
			_userService = userService;
			_entitiesRepository = entitiesRepository;
		}

		public void Init(long[] entitiesIds)
		{
			var socialUserInfos = _userService.GetUsers(entitiesIds);

			if (socialUserInfos.IsNullOrEmpty())
				return;

			var entities = _entitiesRepository.GetEntitiesByIds(socialUserInfos.Select(x => x.id).ToArray())
				.ToArray();

			var initilizers = socialUserInfos.Except(socialUserInfos.Join(entities, z => z.id, e => e.OriginalId, (z, e) => z));

			// TODO: we need to find a way to do this in one batch
			foreach (var entity in initilizers)
			{
				_entitiesRepository.Update(new Entity {
					Id = ObjectId.GenerateNewId(),
					OriginalId = entity.id,
					Name = entity.name,
					ImageUrl = entity.imageUrl,
					OriginalEmail = entity.email
				});
			}
		}
	}
}