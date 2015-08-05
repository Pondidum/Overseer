using System;
using System.Collections.Generic;
using System.Linq;

namespace Overseer
{
	public class ValidationResult
	{
		private readonly List<ValidationResult> _results;

		public ValidationResult(IEnumerable<ValidationResult> results)
		{
			_results = results.ToList();
		}

		public string Message { get {  return _results.Aggregate("", (a, r) => a + Environment.NewLine + r.Message); } }
		public Status Status { get { return _results.Max(r => r.Status); } }
	}
}
