using System;
using System.Collections.Generic;
using System.Linq;
using SYW.App.Pictures.Domain.Entities;

namespace SYW.App.Messages.Web.Services
{
	public interface ISearchFriendsService
	{
		IList<Entity> Search(string query);
	}

	public class SearchFriendsService : ISearchFriendsService
	{
		private readonly ICachedUserFriendsProvider _cachedUserFriendsProvider;
		private readonly IEntityContextProvider _entityContextProvider;
		private readonly ISearchServiceSettings _searchServiceSettings;

		public SearchFriendsService(ICachedUserFriendsProvider cachedUserFriendsProvider, IEntityContextProvider entityContextProvider, ISearchServiceSettings searchServiceSettings)
		{
			_cachedUserFriendsProvider = cachedUserFriendsProvider;
			_entityContextProvider = entityContextProvider;
			_searchServiceSettings = searchServiceSettings;
		}

		/// <summary>
		/// Search friend by name including yourself
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public IList<Entity> Search(string query)
		{
			try
			{
				var friends = _cachedUserFriendsProvider.GetFriends().Concat(new[] {GetCurrentEntity()});

				return friends
					.Where(f => f.Name.ToLower().StartsWith(query.ToLower()))
					.Take(_searchServiceSettings.MaxFriendsSearchResults)
					.ToList();
			}
			catch
			{
				return Enumerable.Empty<Entity>().ToList();
			}
		}

		public Entity GetCurrentEntity()
		{
			var currentEntity = _entityContextProvider.CurrentEntity();

			return new Entity {
				Name = currentEntity.Name,
				Id = currentEntity.Id,
				ImageUrl = currentEntity.ImageUrl,
				OriginalId = currentEntity.OriginalId,
			};
		}
	}
}