using System;
using System.Web.Mvc;
using SYW.App.Messages.Web.Services;
using SYW.App.Pictures.Domain.Entities;
using SYW.App.Pictures.Domain.Services.Platform;

namespace SYW.App.Pictures.Web.Filters
{
	public interface IOfflineTokenProviderFilter : IAuthorizationFilter
	{
	}

	public class OfflineTokenProviderFilter : IOfflineTokenProviderFilter
	{
		private readonly IEntityContextProvider _entityContextProvider;
		private readonly IOfflineTokenProvider _offlineTokenProvider;

		public OfflineTokenProviderFilter(IEntityContextProvider entityContextProvider, IOfflineTokenProvider offlineTokenProvider)
		{
			_entityContextProvider = entityContextProvider;
			_offlineTokenProvider = offlineTokenProvider;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			var attributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(OfflineTokenProviderFilterAttribute), false);
			if (attributes.Length <= 0)
				return;

			var currentEntityContext = _entityContextProvider.CurrentEntity();

			_offlineTokenProvider.UpdateEntityOfflineToken(currentEntityContext.Id);
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OfflineTokenProviderFilterAttribute : ActionFilterAttribute
    {

    }
}