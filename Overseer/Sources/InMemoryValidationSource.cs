using System;
using System.Collections.Generic;
using Overseer.Infrastructure;

namespace Overseer.Sources
{
	public class InMemoryValidationSource : IValidatorSource
	{
		private readonly Cache<string, List<IValidator>> _validators;

		public InMemoryValidationSource()
		{
			_validators = new Cache<string, List<IValidator>>(
				StringComparer.OrdinalIgnoreCase,
				key => new List<IValidator>());
		}

		public void Register(string messageType, IValidator validator)
		{
			_validators[messageType].Add(validator);
		}

		public IEnumerable<IValidator> For(string messageType)
		{
			return _validators[messageType];
		}
	}
}
