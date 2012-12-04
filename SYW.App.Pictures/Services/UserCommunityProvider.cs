using System.Collections.Generic;
using System.Linq;
using SYW.App.Pictures.Domain.Entities;
using SYW.App.Pictures.Domain.Services.Platform;

namespace SYW.App.Messages.Web.Services
{
	public interface IUserCommunityProvider
	{
		IList<Entity> GetFriends();
		IList<Entity> GetFollowedBy();
	}

	public class UserCommunityProvider : IUserCommunityProvider
	{
		private readonly IEntityContextProvider _entityContextProvider;
		private readonly IUserService _userService;

		public UserCommunityProvider(IEntityContextProvider entityContextProvider, IUserService userService)
		{
			_entityContextProvider = entityContextProvider;
			_userService = userService;
		}

		public IList<Entity> GetFriends()
		{
			if (_entityContextProvider.CurrentEntity() == null)
				return new Entity[0];

			var currentEntityId = _entityContextProvider.CurrentEntity().OriginalId;

			var friends = GetFriends(currentEntityId);

			if (friends == null || friends.Any() == false)
				return new Entity[0];

			// TODO : User mapper instead of mapping here
			return friends
				.Select(f => new Entity { OriginalId = f.id, Name = f.name, ImageUrl = f.imageUrl, OriginalEmail = f.email })
				.ToList();
		}

		public IList<Entity> GetFollowedBy()
		{
			if (_entityContextProvider.CurrentEntity() == null)
				return new Entity[0];

			var currentEntityId = _entityContextProvider.CurrentEntity().OriginalId;

			var followedBy = _userService.GetFollowedBy(currentEntityId);

			if (followedBy == null || followedBy.Count == 0)
				return new Entity[0];

			var friends = _userService.GetUsers(followedBy);

			if (friends == null || friends.Any() == false)
				return new Entity[0];

			return friends
				.Select(f => new Entity { OriginalId = f.id, Name = f.name, ImageUrl = f.imageUrl, OriginalEmail = f.email })
				.ToList();
		}

		private List<SocialUserInfo> GetUsers(IList<long> followedBy)
		{
			const int numUsersPerQuery = 150;

			if (followedBy.Count <= numUsersPerQuery)
				return _userService.GetUsers(followedBy);

			var users = new List<SocialUserInfo>();
			var userCount = 0;
			while (userCount < followedBy.Count)
			{
				var usersToQuery = followedBy.Count - userCount < numUsersPerQuery ? followedBy.Count - userCount : numUsersPerQuery;
				var usersToAdd = _userService.GetUsers(followedBy.Skip(userCount).Take(usersToQuery).ToList());
				if (usersToAdd != null && usersToAdd.Any())
					users = users.Concat(usersToAdd).ToList();
				userCount += usersToQuery;
			}

			return users;
		}

		private List<SocialUserInfo> GetFriends(long currentEntityId)
		{
			var followedBy = _userService.GetFollowedBy(currentEntityId);
			var followers = _userService.GetFollowers(currentEntityId);

			var friendsIds = followedBy.Intersect(followers).ToList();

			if (friendsIds.Count == 0)
				return null;

			return _userService.GetUsers(friendsIds);
		}
	}
}