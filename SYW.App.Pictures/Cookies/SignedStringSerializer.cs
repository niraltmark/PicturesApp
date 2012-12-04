using System;

namespace SYW.App.Messages.Web.Cookies
{
	class SignedStringSerializer : SignedValueSerializer<string>
	{
		public SignedStringSerializer(TimeSpan timeToLive, ICryptoService cryptoService)
			: base(timeToLive, null, cryptoService)
		{
		}

		protected override string DeserializeObject(object value)
		{
			return value.ToString();
		}
	}
}