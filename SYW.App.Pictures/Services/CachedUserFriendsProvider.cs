using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using CommonGround.Utilities;
using SYW.App.Pictures.Domain.DataAccess.Entities;
using SYW.App.Pictures.Domain.Entities;

namespace SYW.App.Messages.Web.Services
{
	public interface ICachedUserFriendsProvider
	{
		IList<Entity> GetFriends();
		void Init();
	}

	public class CachedUserFriendsProvider : ICachedUserFriendsProvider
	{
		private readonly IEntityContextProvider _entityContextProvider;
		private readonly IEntityNetworkRepository _entityNetworkRepository;
		private readonly IUserCommunityProvider _userCommunityProvider;
		private readonly IHttpContextProvider _httpContextProvider;

		public CachedUserFriendsProvider(IEntityContextProvider entityContextProvider, IUserCommunityProvider userCommunityProvider, IHttpContextProvider httpContextProvider, IEntityNetworkRepository entityNetworkRepository)
		{
			_entityContextProvider = entityContextProvider;
			_userCommunityProvider = userCommunityProvider;
			_httpContextProvider = httpContextProvider;
			_entityNetworkRepository = entityNetworkRepository;
		}

		public IList<Entity> GetFriends()
		{
			var friends = _httpContextProvider.GetContext().Cache.Get(FriendsCacheKey) as IList<Entity>;

			if (friends != null && friends.Any())
				return friends;

			friends = GetNetwork();

			CacheEntityNetwork(friends);

			return friends;
		}

		public void Init()
		{
			var friends = _userCommunityProvider.GetFollowedBy();
			
			CacheEntityNetwork(friends);

			_entityNetworkRepository.UpdateNetwork(_entityContextProvider.CurrentEntity().Id, friends.ToArray());
		}

		private void CacheEntityNetwork(IList<Entity> friends)
		{
			_httpContextProvider.GetContext().Cache.Add(FriendsCacheKey, friends, null,
			                                            SystemTime.Now().AddMinutes(1),
			                                            Cache.NoSlidingExpiration,
			                                            CacheItemPriority.High, null);
		}

		private IList<Entity> GetNetwork()
		{
			var entityId = _entityContextProvider.CurrentEntity().Id;

			var network = _entityNetworkRepository.GetNetwork(entityId);

			if (network != null)
				return network.Friends;

			var friends = _userCommunityProvider.GetFollowedBy();

			_entityNetworkRepository.UpdateNetwork(entityId, friends.ToArray());

			return friends;
		}

		private string FriendsCacheKey
		{
			get
			{
				var currentEntityId = _entityContextProvider.CurrentEntity().OriginalId;

				return "FriendsCache_" + currentEntityId;
			}
		}
	}
}