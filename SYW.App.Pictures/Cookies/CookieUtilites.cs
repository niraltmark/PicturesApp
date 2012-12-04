using System;
using System.Web;

namespace SYW.App.Messages.Web.Cookies
{
	public static class CookieUtilites
	{
		public static string EncodeCookieValue(string value)
		{
			return HttpUtility.UrlEncode(value);
		}

		public static string DecodeCookieValue(string value)
		{
			return HttpUtility.UrlDecode(value);
		}

		public static void RemoveCookie(string cookieName)
		{
			var httpContext = HttpContext.Current;
			if (httpContext == null)
				return;

			var cookie = httpContext.Request.Cookies[cookieName];
			if (cookie == null)
				return;

			cookie.Expires = DateTime.Now.AddYears(-1);
			httpContext.Response.Cookies.Set(cookie);
		}
	}
}