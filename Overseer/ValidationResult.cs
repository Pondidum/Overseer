using System.Collections.Generic;
using System.Linq;

namespace Overseer
{
	public class ValidationResult
	{
		protected ValidationResult()
		{
			Results = Enumerable.Empty<ValidationResult>();
		}

		public IEnumerable<ValidationResult> Results { get; protected set; }
		public Status Status { get; protected set; }
		public string Message { get; protected set; }
	}
}
