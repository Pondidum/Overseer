using System;
using System.Collections.Generic;
using System.Linq;
using Overseer.Infrastructure;

namespace Overseer.Sources
{
	public class CachingValidatorSource : IValidatorSource
	{
		private readonly Cache<string, List<IValidator>> _cache;

		public CachingValidatorSource(IValidatorSource other)
		{
			if (other== null) throw new ArgumentNullException("other");

			_cache = new Cache<string, List<IValidator>>(
				StringComparer.OrdinalIgnoreCase, 
				key => other.For(key).ToList());
		}

		public IEnumerable<IValidator> For(string messageType)
		{
			return _cache[messageType];
		}
	}
}
