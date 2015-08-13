using System;
using System.Collections;
using System.Collections.Generic;

namespace Overseer.Infrastructure
{
	public class Cache<TKey, TValue> : IEnumerable<TValue>
	{
		private readonly Func<TKey, TValue> _onMissing;
		private readonly object _locker = new object();
		private readonly Dictionary<TKey, TValue> _cache;

		public Cache(Func<TKey, TValue> onMissing)
			: this(null, onMissing)
		{
		}

		public Cache(IEqualityComparer<TKey> comparer, Func<TKey, TValue> onMissing)
		{
			_onMissing = onMissing;
			_cache = new Dictionary<TKey, TValue>(comparer);
		}

		public TValue this[TKey key]
		{
			get
			{
				Fill(key);
				return _cache[key];
			}
		}

		public void Fill(TKey key)
		{
			if (_cache.ContainsKey(key))
			{
				return;
			}

			lock (_locker)
			{
				if (!_cache.ContainsKey(key))
				{
					_cache.Add(key, _onMissing(key));
				}
			}
		}

		public IEnumerator<TValue> GetEnumerator()
		{
			return _cache.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}