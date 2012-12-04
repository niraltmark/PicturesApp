using System;
using SYW.App.Pictures.Web.Cookies;

namespace SYW.App.Messages.Web.Cookies
{
	public class StateProvider : ContextAwareComponent, IStateProvider
	{
		public StateProvider(IEngineContextLocator contextLocator)
			: base(contextLocator)
		{
		}
		public IState<string> CookieState(string cookieName, TimeSpan timeToLive, bool httpOnly, bool supportP3P)
		{
			var cookieState = new CookieState(ContextLocator, cookieName, timeToLive, httpOnly, supportP3P);

			// We also want a context item cache for the cookie value, so that it can be read on the same request context as when it was set
			var wrapper = new ContextItemStateWrapper<string>(ContextLocator, "cookieCache." + cookieName, cookieState);
			return wrapper;
		}
	}
}