using System.Collections.Generic;

namespace Overseer
{
	public class ValidationResult
	{
		protected ValidationResult()
		{
		}

		public IEnumerable<ValidationResult> Results { get; protected set; }
		public Status Status { get; protected set; }
		public string Message { get; protected set; }
	}
}
