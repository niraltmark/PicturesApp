using System;
using System.Collections.Generic;
using System.Linq;

namespace SYW.App.Pictures.Domain
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Determines whether the sequence is null or contains no elements.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> target)
		{
			return target == null || !target.Any();
		}

		/// <summary>
		/// Replaces null with an empty sequence.
		/// </summary>
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> target)
		{
			return target ?? Enumerable.Empty<T>();
		}

		/// <summary>
		/// Consumes the sequence by iterating over it. 
		/// </summary>
		public static void Consume<T>(this IEnumerable<T> target)
		{
			foreach (var item in target)
			{
				// nop, we just want to iterate over the sequence
			}
		}

		/// <summary>
		/// Correlates the elements in two sequences according to a shared key, then performs an action for each matching pair.
		/// </summary>
		public static void JoinDo<TLeft, TRight, TKey>(this IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TKey> leftKeySelector, Func<TRight, TKey> rightKeySelector, Action<TLeft, TRight> matchAction)
		{
			left.Join(right, leftKeySelector, rightKeySelector, (l, r) =>
			{
				matchAction(l, r);
				return 0;
			}).Consume();
		}
	}
}