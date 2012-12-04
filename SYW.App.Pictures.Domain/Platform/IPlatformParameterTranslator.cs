using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PlatformClient.Platform
{
	public interface IPlatformParameterTranslator
	{
		string TranslateParameters(KeyValuePair<string, object> parameter);
	}

	public class PlatformParameterTranslator : IPlatformParameterTranslator
	{
		private readonly Type[] _untypedCollectionInterfaces = new[] { typeof(IEnumerable), typeof(IList), typeof(ICollection) };
		private readonly Type[] _genericCollectionInterfaces = new[] { typeof(IEnumerable<>), typeof(IList<>), typeof(ICollection<>) };

		private bool IsCollectionInstance(Type type)
		{
			if (type == typeof(string)) return false;
			return type.GetInterfaces().Any(typeToCheck => _untypedCollectionInterfaces.Any(untypedInteface => typeToCheck == untypedInteface) || _genericCollectionInterfaces.Any(typedInterface => typedInterface == typeToCheck));
		}

		public string TranslateParameters(KeyValuePair<string, object> parameter)
		{
			var parameterType = parameter.Value.GetType();

			if (parameterType.IsValueType || parameterType == typeof(string))
			{
				return parameter.Value.ToString();
			}

			if (parameterType.IsArray || IsCollectionInstance(parameterType))
			{
				var objectArray = ((IEnumerable)parameter.Value).Cast<object>().Select(x => x.ToString());
				var stringifiedArray = String.Join(",", objectArray.ToArray());
				return stringifiedArray;

			}

			return JsonConvert.SerializeObject(parameter.Value);
		}
	}
}
