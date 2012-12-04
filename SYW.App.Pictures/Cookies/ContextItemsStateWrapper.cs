using SYW.App.Pictures.Web.Cookies;

namespace SYW.App.Messages.Web.Cookies
{
	/// <summary>
	/// Provides a context-item-based cache for other types of states (e.g. CookieState).
	/// </summary>
	/// <remarks>
	/// This class is used to provide a context-item-based cache for cookies, in order to allow
	/// reading the cookie during the same request in which it was set and obtaining the "updated" value.
	/// </remarks>
	internal class ContextItemStateWrapper<T> : ContextAwareComponent, IState<T>
	{
		private readonly string _key;
		private readonly IState<T> _underlying;

		public ContextItemStateWrapper(IEngineContextLocator contextLocator, string key, IState<T> underlyingState)
			: base(contextLocator)
		{
			_key = key;
			_underlying = underlyingState;
		}

		public T Get()
		{
			if (Context.Items.Contains(_key)) return (T)Context.Items[_key];

			var value = _underlying.Get();
			return value;
		}

		public void Set(T value)
		{
			Context.Items[_key] = value;
			_underlying.Set(value);
		}
	}
}