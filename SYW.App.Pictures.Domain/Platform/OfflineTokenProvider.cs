using MongoDB.Bson;
using SYW.App.Pictures.Domain.DataAccess.Entities;

namespace SYW.App.Pictures.Domain.Services.Platform
{
	public interface IOfflineTokenProvider
	{
		void UpdateEntityOfflineToken(ObjectId id);
	}

	public class OfflineTokenProvider : IOfflineTokenProvider
	{
		private readonly IEntitiesRepository _entitiesRepository;
		private readonly IAuthService _authService;

		public OfflineTokenProvider(IEntitiesRepository entitiesRepository, IAuthService authService)
		{
			_entitiesRepository = entitiesRepository;
			_authService = authService;
		}

		public void UpdateEntityOfflineToken(ObjectId id)
		{
			// TODO : Extract to other places, we might want to use it
			var entity = _entitiesRepository.Get(id);

			if (!string.IsNullOrWhiteSpace(entity.PlatformOfflineToken))
			    return;

			var offlineToken = _authService.GetOfflineToken(entity.OriginalId);

			entity.PlatformOfflineToken = offlineToken;

			_entitiesRepository.Update(entity);
		}
	}
}