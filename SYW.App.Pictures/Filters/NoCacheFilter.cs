using System.Web;
using System.Web.Mvc;

namespace SYW.App.Pictures.Web.Filters
{
	public interface INoCacheFilter : IAuthorizationFilter
	{
		
	}

	public class NoCacheFilter : INoCacheFilter
	{
		public void OnAuthorization(AuthorizationContext filterContext)
		{
			filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
		}
	}
}