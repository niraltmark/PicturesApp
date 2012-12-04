using System;
using System.Web.Mvc;
using HelloApps.Services;
using PlatformClient.Platform;
using SYW.App.Messages.Web.Services;
using SYW.App.Pictures.Domain.Services.Platform;

namespace SYW.App.Pictures.Web.Filters
{
	public interface ITokenExtractorFilter : IAuthorizationFilter
	{
	}

	public class TokenExtractorFilter : ITokenExtractorFilter
	{
		private readonly IPlatformTokenProvider _platformTokenProvider;
		private readonly IEntityContextProvider _entityContextProvider;

		public TokenExtractorFilter(IPlatformTokenProvider platformTokenProvider, IEntityContextProvider entityContextProvider, IAppService appService)
		{
			_platformTokenProvider = platformTokenProvider;
			_entityContextProvider = entityContextProvider;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			var ignoreAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreTokenExtractor), false);
            if (ignoreAttributes.Length > 0)
                return;

			var token = filterContext.HttpContext.Request.QueryString["token"];

			// This mean that we arrived from a page via the platform
			if (!string.IsNullOrEmpty(token))
			{
				// Set the token for platform request
				_platformTokenProvider.SetToken(token);

				// If the cookie authentication cookie exist we need to clear it in order to reset it
				_entityContextProvider.Clear();
			}
			// This mean that we are going through internal pages and we need to get the info from the cookie
			else
			{
				// TODO : Need to think how to hanlde cases where the cookie expired
				var entityContext = _entityContextProvider.CurrentEntity();

				// Set the token for platform request
				_platformTokenProvider.SetToken(entityContext.Token);
			}
		}
	}

	public class InvalidTokenException : Exception
	{
		public InvalidTokenException(string myMessage)
			: base(myMessage)
		{

		}
	}

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreTokenExtractor : ActionFilterAttribute
    {

    }

}