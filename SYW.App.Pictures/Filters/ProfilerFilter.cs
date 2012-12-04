using System.Web;
using System.Web.Mvc;
using HelloApps.Services;
using PlatformClient.Platform;
using SYW.App.Messages.Web.Services;
// using StackExchange.Profiling;

namespace SYW.App.Pictures.Web.Filters
{
//    public interface IProfilerFilter : IAuthorizationFilter
//    {
//    }

//    public class ProfilerFilter : IProfilerFilter
//    {
//        public void OnAuthorization(AuthorizationContext filterContext)
//        {
//            var value = filterContext.HttpContext.Request.QueryString["profiler"];

//            if (!string.IsNullOrEmpty(value))
//            {
//                StartProfiler(filterContext, value);
//            }
//            else
//            {
//                var cookie = filterContext.HttpContext.Request.Cookies["profiler"];

//                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
//                {
//                    StartProfiler(filterContext, cookie.Value);
//                }
//            }
//        }

//        private static void StartProfiler(AuthorizationContext filterContext, string value)
//        {
//            bool profiler;
//            bool.TryParse(value, out profiler);

//            if (profiler)
//            {
//                filterContext.HttpContext.Response.Cookies.Set(new HttpCookie("profiler", "true"));

//                MiniProfiler.Start();
//            }
//        }
//    }
}