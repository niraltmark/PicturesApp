using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SYW.App.Messages.Web.Cookies
{
	class JsonSerializer<T> : ISerializer<T, string>
	{
		private readonly T _missingValue;

		private static readonly JsonSerializer _serializer = new JsonSerializer
		{
		    ContractResolver = new CamelCasePropertyNamesContractResolver(),
		    DefaultValueHandling = DefaultValueHandling.Ignore,
		    NullValueHandling = NullValueHandling.Ignore,
		    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		    MissingMemberHandling = MissingMemberHandling.Ignore,
		};

		public JsonSerializer()
			: this(default(T))
		{
		}

		public JsonSerializer(T missingValue)
		{
			_missingValue = missingValue;
		}

		public string Serialize(T value)
		{
			if (Equals(value, _missingValue))
				return null;

			var sb = new StringBuilder();
			_serializer.Serialize(new StringWriter(sb), value);
			return sb.ToString();
		}

		public T Deserialize(string serialized)
		{
			if (serialized == null) return _missingValue;

			try
			{
				return _serializer.Deserialize<T>(new JsonTextReader(new StringReader(serialized)));
			}
			catch
			{
				return _missingValue;
			}
		}
	}
}