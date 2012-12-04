using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SYW.App.Pictures.Domain;
using SYW.App.Pictures.Domain.Services;
using SYW.App.Pictures.Domain.Services.Settings;
using log4net;

namespace SYW.App.Pictures.Web.Filters
{
	public class ExceptionFilter : IExceptionFilter
	{
		private static readonly JsonSerializerSettings JsonSerializationSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			DefaultValueHandling = DefaultValueHandling.Ignore
		};

		private readonly ILog _log;
		private readonly IApplicationSettings _applicationSettings;
		private readonly IRoutes _routes;

		public ExceptionFilter(ILog log, IApplicationSettings applicationSettings, IRoutes routes)
		{
			_log = log;
			_applicationSettings = applicationSettings;
			_routes = routes;
		}

		public void OnException(ExceptionContext filterContext)
		{
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
			filterContext.ExceptionHandled = true;

			HandleError(filterContext);

			if (filterContext.HttpContext.Request.IsAjaxRequest())
				return;

			if (filterContext.Exception is UnauthorizedOperationException)
			{
				var siteUrl = _applicationSettings.BaseSiteUrl + _routes.DefaultAppUrl;
				filterContext.HttpContext.Response.StatusDescription = "UNAUTHORIZED";
				filterContext.Result = new ContentResult { Content = "<script>window.top.location.href = '" + siteUrl + "'</script>" };
			}
			else
			{
				filterContext.HttpContext.Response.StatusDescription = "ERROR";
				filterContext.Result = CreateActionResult(filterContext);
			}
		}

		private void HandleError(ExceptionContext filterContext)
		{
			filterContext.HttpContext.Response.StatusCode = 500;

			_log.Error(filterContext.Exception.Message, filterContext.Exception);

			if (filterContext.HttpContext.Request.IsAjaxRequest() == false)
				return;

			ConvertExceptionToJson(filterContext.HttpContext, 500, filterContext.Exception.Message);
		}

		private static void ConvertExceptionToJson(HttpContextBase context, int statusCode, string message)
		{
			context.Response.ContentType = "application/json";
			var guid = context.Items["RequestGuid"] as Guid?;

			var responseObj = new
			{
				Error = new
				{
					StatusCode = statusCode,
					Message = message,
					IncidentId = guid.HasValue ? guid.Value.ToString() : null
				}
			};

			context.Response.Write(JsonConvert.SerializeObject(responseObj, Formatting.None, JsonSerializationSettings));
		}

		protected virtual ActionResult CreateActionResult(ExceptionContext filterContext)
		{
			var controllerName = (string)filterContext.RouteData.Values["controller"];
			var actionName = (string)filterContext.RouteData.Values["action"];
			var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
			var result = new ViewResult
			{
				ViewName = "Error",
				ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
			};

			result.ViewBag.StatusCode = filterContext.HttpContext.Response.StatusCode;

			return result;
		}
	}
}