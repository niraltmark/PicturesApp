using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SYW.App.Messages.Web.Cookies
{
	/// <summary>
	/// Provides wrapper extensions around <see cref="IState{T}"/> objects.
	/// </summary>
	public static class StateExtensions
	{
		/// <summary>
		/// Ensures the state is signed when written, and the signature verified when read, so as to prevent tampering.
		/// </summary>
		/// This merely causes the data to be signed, not encrypted; If the state is stored in a cookie, its values can still be read by users.
		public static IState<string> Signed(this IState<string> target, ICryptoService cryptoService, TimeSpan timeToLive)
		{
			return target.Serialized(new SignedStringSerializer(timeToLive, cryptoService));
		}

		/// <summary>
		/// Ensures the state is signed when written, and the signature verified when read, so as to prevent tampering.
		/// </summary>
		/// This merely causes the data to be signed, not encrypted; If the state is stored in a cookie, its values can still be read by users.
		public static IState<IDictionary<string, object>> Signed(this IState<IDictionary<string, object>> target,
		                                                         ICryptoService cryptoService, TimeSpan timeToLive)
		{
			return target.Serialized(new SignedDictionarySerializer(timeToLive, cryptoService));
		}

		/// <summary>
		/// Provides a typed wrapper around an existing state mechanism. 
		/// </summary>
		/// <typeparam name="T">The desired state type.</typeparam>
		/// <param name="target">The target state to wrap.</param>
		/// <param name="missingValue">The value to use when the underlying state value is not found.</param>
		/// <returns>A typed state instance over the target.</returns>
		public static IState<T> Jsoned<T>(this IState<string> target, T missingValue)
		{
			return target.Serialized(new JsonSerializer<T>(missingValue));
		}

		/// <summary>
		/// Provides a typed wrapper around an existing state mechanism. 
		/// </summary>
		/// <typeparam name="T">The desired state type.</typeparam>
		/// <param name="target">The target state to wrap.</param>
		/// <param name="missingValue">The value to use when the underlying state value is not found.</param>
		/// <returns>A typed state instance over the target.</returns>
		/// This method uses the desired type's default value when the underlying state value is not found.
		public static IState<T> Jsoned<T>(this IState<string> target)
		{
			return Jsoned(target, default(T));
		}

		public static IState<IDictionary<string, object>> Dictionarized(this IState<string> target)
		{
			return target.Serialized(new JsonSerializer<IDictionary<string, object>>(null));
		}

		public static IState<T> Serialized<T>(this IState<string> target,
		                                      ISerializer<T, IDictionary<string, object>> dictionarySerializer)
		{
			return target.Dictionarized().Serialized(dictionarySerializer);
		}

		public static IState<TOut> Serialized<TIn, TOut>(this IState<TIn> target, ISerializer<TOut, TIn> serializer)
		{
			return new StateAdapter<TOut, TIn>(target, serializer);
		}
	}
}
