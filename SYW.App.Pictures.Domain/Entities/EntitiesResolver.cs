using System.Collections.Generic;
using System.Linq;
using SYW.App.Pictures.Domain.DataAccess.Entities;

namespace SYW.App.Pictures.Domain.Entities
{
	public interface IEntitiesResolver
	{
		void Resolve(IList<Entity> unresolvedEntities);
	}

	public class EntitiesResolver : IEntitiesResolver
	{
		private readonly IEntitiesRepository _entitiesRepository;

		public EntitiesResolver(IEntitiesRepository entitiesRepository)
		{
			_entitiesRepository = entitiesRepository;
		}

		public void Resolve(IList<Entity> unresolvedEntities)
		{
			var ids = unresolvedEntities.Select(e => e.Id).ToArray();

			var resolvedEntities = _entitiesRepository.GetEntitiesByIds(ids)
				.Where(e => e != null);

			unresolvedEntities.JoinDo(resolvedEntities, u => u.Id, r => r.Id, (u,r) =>
			{
				u.OriginalId = r.OriginalId;
				u.ImageUrl = r.ImageUrl;
				u.Name = r.Name;
				u.OriginalEmail = r.OriginalEmail;
				u.Email = r.Email;
			});
		}
	}
}