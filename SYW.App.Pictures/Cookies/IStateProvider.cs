using System;

namespace SYW.App.Messages.Web.Cookies
{
	public interface IState<T>
	{
		T Get();
		void Set(T value);
	}

	public interface IStateProvider
	{
		IState<string> CookieState(string cookieName, TimeSpan timeToLive, bool httpOnly, bool supportP3P);
	}
}