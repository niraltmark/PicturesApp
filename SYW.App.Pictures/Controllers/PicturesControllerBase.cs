using System.Web;
using System.Web.Mvc;

namespace SYW.App.Pictures.Controllers
{
	public class PicturesControllerBase : CommonGround.MvcInvocation.ControllerBase
	{
		protected ViewResult View<T>(string viewName, T model)
		{
			//if (HttpContext != null && HttpContext.Request.IsMobileDevice())
			//    return base.View(viewName, "_Mobile", model);

			return base.View(viewName, model);
		}
	}

	public static class HttpRequestExtensions
	{
		public static bool IsMobileDevice(this HttpRequestBase request)
		{
			if (request.Browser.IsMobileDevice)
				return true;

			if (string.IsNullOrEmpty(request.UserAgent))
				return false;

			return request.UserAgent.ToLower().Contains("iphone");
		}
	}
}