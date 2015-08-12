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
			Status = results.Select(r => r.Status).DefaultIfEmpty().Max();
			Message = string.Empty;
		}
	}
}
