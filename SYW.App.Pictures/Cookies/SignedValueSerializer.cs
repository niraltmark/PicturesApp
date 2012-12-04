using System;
using System.Collections.Generic;
using CommonGround.Utilities;
using Newtonsoft.Json;

namespace SYW.App.Messages.Web.Cookies
{
	abstract class SignedValueSerializer<T> : ISerializer<T, IDictionary<string, object>>
	{
		private const string ValueKey = "v";
		private const string SignatureKey = "s";
		private const string TimeKey = "t";

		private readonly ICryptoService _cryptoService;
		private readonly TimeSpan _timeToLive;
		private readonly T _missingValue;

		protected SignedValueSerializer(TimeSpan timeToLive, T missingValue, ICryptoService cryptoService)
		{
			_timeToLive = timeToLive;
			_missingValue = missingValue;
			_cryptoService = cryptoService;
		}

		public IDictionary<string, object> Serialize(T value)
		{
			if (Equals(value, _missingValue))
			{
				return null;
			}

			// We sign the json-serialization of the value
			var signature = _cryptoService.Sign(JsonConvert.SerializeObject(value));

			return new Dictionary<string, object>
			       	{
			       		{ ValueKey, value },
			       		{ SignatureKey, signature },
			       		{ TimeKey, SystemTime.Now() }
			       	};
		}

		public T Deserialize(IDictionary<string, object> serialized)
		{
			if (serialized == null) return _missingValue;

			try
			{
				var time = (DateTime)serialized[TimeKey];
				if (_timeToLive != TimeSpan.Zero && SystemTime.Now() - time > _timeToLive)
					return _missingValue;

				var value = DeserializeObject(serialized[ValueKey]);
				var signature = (string)serialized[SignatureKey];
				if (!_cryptoService.VerifySignature(JsonConvert.SerializeObject(value), signature))
					return _missingValue;

				return value;
			}
			catch
			{
				return _missingValue;
			}
		}

		protected abstract T DeserializeObject(object value);
	}
}