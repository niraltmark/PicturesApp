using System;
using CommonGround.Utilities;
using SYW.App.Messages.Web.Cookies;
using SYW.App.Pictures.Domain.Entities;
using SYW.App.Pictures.Web.Filters;

namespace SYW.App.Messages.Web.Services
{
	public interface IEntityContextProvider
	{
		EntityContext CurrentEntity();
		void SetToken(string token);
		void Clear();
	}

	public class EntityContextProvider : IEntityContextProvider
	{
		private readonly IEntityProvider _entityProvider;
		private readonly IState<EntityContext> _entityContext;

		public EntityContextProvider(IEntityProvider entityProvider, IStateProvider stateProvider, ICryptoService cryptoService)
		{
			_entityProvider = entityProvider;

			_entityContext = stateProvider.CookieState("authctx", TimeSpan.Zero, false, true)
				.Signed(cryptoService, TimeSpan.Zero)
				.Jsoned<EntityContext>();
		}

		public EntityContext CurrentEntity()
		{
			var entityContext = GetEntityContext();

			SetEntityContext(entityContext);

			return entityContext;
		}

		public void SetToken(string token)
		{
			var entityContext = GetEntityContext();

			entityContext.Token = token;

			SetEntityContext(entityContext);
		}

		public void Clear()
		{
			_entityContext.Set(null);
		}

		private EntityContext GetEntityContext()
		{
			var entityContext = _entityContext.Get();

			if (entityContext != null)
				return entityContext;

			var entity = _entityProvider.GetEntityFromPlatform();

			if (entity == null)
				throw new UnauthorizedOperationException();

			return new EntityContext
			{
				ObjectId = entity.Id.ToString(),
				OriginalId = entity.OriginalId,
				Name = entity.Name,
				ImageUrl = entity.ImageUrl,
				EntityType = EntityType.User,
				MemberSince = entity.MemberSince == null ? SystemTime.Now() : entity.MemberSince.Value
			};
		}

		private void SetEntityContext(EntityContext entityContext)
		{
			_entityContext.Set(entityContext);
		}
	}
} 