using System;
using System.Collections.Generic;
using System.Linq;

namespace Overseer
{
	public class ValidationResultNode : ValidationResult
	{

		public ValidationResultNode(List<ValidationResult> results)
		{
			Results = results;
			Status = results.Max(r => r.Status);
			Message = results.Aggregate("", (a, r) => a + Environment.NewLine + r.Message);
		}
	}
}
