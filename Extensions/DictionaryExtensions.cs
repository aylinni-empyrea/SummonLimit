using System;
using System.Collections.Generic;
using System.Linq;

namespace SummonLimit
{
	internal static class DictionaryExtensions
	{
		/// <summary>
		///   Removes all elements matching the predicate method.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dic">Dictionary to remove keys from.</param>
		/// <param name="predicate">A predicate <see cref="Func{TKey, TValue, TResult}" /> that returns a <see cref="bool" />.</param>
		internal static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TKey, TValue, bool> predicate)
		{
			var keys = dic.Keys.Where(k => predicate(k, dic[k])).ToList();
			foreach (var key in keys)
				dic.Remove(key);
		}
	}
}