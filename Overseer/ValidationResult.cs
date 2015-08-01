using System.Collections.Generic;

namespace Overseer
{
	public class ValidationResult
	{
		public IEnumerable<string> Messages { get; set; }
		public ValidationStatus Status { get; set; }
	}
}
