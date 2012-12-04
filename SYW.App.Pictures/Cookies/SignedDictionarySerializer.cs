using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SYW.App.Messages.Web.Cookies
{
	class SignedDictionarySerializer : SignedValueSerializer<IDictionary<string, object>>
	{
		public SignedDictionarySerializer(TimeSpan timeToLive, ICryptoService cryptoService)
			: base(timeToLive, null, cryptoService)
		{
		}

		protected override IDictionary<string, object> DeserializeObject(object value)
		{
			return JsonConvert.DeserializeObject<IDictionary<string, object>>(value.ToString());
		}
	}
}