using System.Web;

namespace SYW.App.Messages.Web.Cookies
{
	public static class CookiesExtensions
	{
		public static string ReadCookie(this HttpRequestBase request, string cookieName)
		{
			var cookie = request.Cookies.Get(cookieName);
			if (cookie == null)
				return null;

			return cookie.Value;
		}

		public static void CreateCookie(this HttpResponseBase response, string name, string value)
		{
			CreateCookie(response, new HttpCookie(name, value));
		}

		public static void CreateCookie(this HttpResponseBase response, HttpCookie cookie)
		{
			response.Cookies.Set(cookie);
		}

		public static void RemoveCookie(this HttpResponseBase response, string cookieName)
		{
			response.Cookies.Remove(cookieName);
		}
	}
}