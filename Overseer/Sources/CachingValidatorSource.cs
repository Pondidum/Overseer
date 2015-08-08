using System;
using System.Collections.Generic;
using System.Linq;

namespace Overseer.Sources
{
	public class CachingValidatorSource : IValidatorSource
	{
		private readonly object _locker;
		private readonly Dictionary<string, List<IValidator>> _cache;
		private readonly Func<string, List<IValidator>> _onMissing;

		public CachingValidatorSource(IValidatorSource other)
		{
			if (other== null) throw new ArgumentNullException("other");

			_locker = new object();
			_cache = new Dictionary<string, List<IValidator>>();
			_onMissing = key => other.For(key).ToList();
		}

		public IEnumerable<IValidator> For(string messageType)
		{
			Fill(messageType);
			return _cache[messageType];
		}
		
		public void Fill(string key)
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
	}
}
