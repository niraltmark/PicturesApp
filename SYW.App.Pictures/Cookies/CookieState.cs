using System;
using System.Web;
using CommonGround.Utilities;
using SYW.App.Pictures.Web.Cookies;

namespace SYW.App.Messages.Web.Cookies
{
	internal class CookieState : ContextAwareComponent, IState<string>
	{
		private readonly string _cookieName;
		private readonly bool _httpOnly;
		private readonly bool _supportP3P;

		/// <summary>
		/// Creates a cookie state mechanism.
		/// </summary>
		public CookieState(IEngineContextLocator contextLocator, string cookieName, TimeSpan timeToLive, bool httpOnly, bool supportP3P)
			: base(contextLocator)
		{
			_cookieName = cookieName;
			_httpOnly = httpOnly;
			_supportP3P = supportP3P;
			TimeToLive = timeToLive;
		}

		public string Get()
		{
			var value = Context.Request.ReadCookie(_cookieName);
			if (!string.IsNullOrEmpty(value))
				value = CookieUtilites.DecodeCookieValue(value);
			return value;
		}

		public void Set(string value)
		{
			if (value == null)
			{
				CookieUtilites.RemoveCookie(_cookieName);
				return;
			}

			value = CookieUtilites.EncodeCookieValue(value);

			if (_supportP3P)
				Context.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

			Context.Response.CreateCookie(new HttpCookie(_cookieName, value)
			{
				Expires = GetExpirationDate(),
				HttpOnly = _httpOnly,
				Path = ApplicationRootPath // All cookies are root-pathed for now
			});
		}

		protected string ApplicationRootPath
		{
			get { return "/"; }
		}

		/// <summary>
		/// Computes the date at which the cookie should expire.
		/// </summary>
		/// return default(DateTime) from this method to set a session-scoped cookie.
		protected DateTime GetExpirationDate()
		{
			return TimeToLive == TimeSpan.Zero ? default(DateTime) : SystemTime.Now() + TimeToLive;
		}

		public TimeSpan TimeToLive { get; set; }

		public string Path { get; set; }
	}
}