namespace SYW.App.Messages.Web.Cookies
{
	class StateAdapter<T, TSerialized> : IState<T>
	{
		private readonly IState<TSerialized> _underlyingState;
		private readonly ISerializer<T, TSerialized> _serializer;

		public StateAdapter(IState<TSerialized> underlyingState, ISerializer<T, TSerialized> serializer)
		{
			_underlyingState = underlyingState;
			_serializer = serializer;
		}

		public T Get()
		{
			return _serializer.Deserialize(_underlyingState.Get());
		}

		public void Set(T value)
		{
			_underlyingState.Set(_serializer.Serialize(value));
		}
	}
}