using System;
using System.Web.Mvc;
using SYW.App.Messages.Web.Services;
using SYW.App.Pictures.Domain.Services.Platform;

namespace SYW.App.Pictures.Web.Filters
{
	public interface IAutoLoginFilter : IAuthorizationFilter
	{
	}

	public class AutoLoginFilter : IAutoLoginFilter
	{
		private readonly IPlatformTokenProvider _platformTokenProvider;
		private readonly IEntityContextProvider _entityContextProvider;

		public AutoLoginFilter(IPlatformTokenProvider platformTokenProvider, IEntityContextProvider entityContextProvider)
		{
			_platformTokenProvider = platformTokenProvider;
			_entityContextProvider = entityContextProvider;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			var token = _platformTokenProvider.GetToken();

			var ignoreAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreAutoLogin), false);
			if (ignoreAttributes.Length > 0)
				return;

			var entityContext = _entityContextProvider.CurrentEntity();

			if (entityContext.Token != token)
				_entityContextProvider.SetToken(token);
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class IgnoreAutoLogin : ActionFilterAttribute
	{
		
	}
}