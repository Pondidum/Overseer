using System.Collections.Generic;

namespace Overseer.Outputs
{
	public class InMemoryValidationOutput : IValidationOutput
	{
		private readonly List<ValidationResult> _results;

		public InMemoryValidationOutput()
		{
			_results = new List<ValidationResult>();
		}

		public IEnumerable<ValidationResult> Results { get { return _results; } }
 
		public void Write(ValidationResult result)
		{
			_results.Add(result);
		}
	}
}
