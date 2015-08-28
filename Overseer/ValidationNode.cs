using System.Collections.Generic;
using System.Linq;

namespace Overseer
{
	public class ValidationNode
	{
		public Status Status { get; protected set; }
		public string ValidationMessage { get; protected set; }
		public IEnumerable<ValidationNode> Children { get; protected set; }

		public ValidationNode(Status status, string validationMessage)
		{
			Status = status;
			ValidationMessage = validationMessage;
			Children = Enumerable.Empty<ValidationNode>();
		}

		public ValidationNode(string validationMessage, IEnumerable<ValidationNode> nodes)
		{
			var children = nodes.ToList();

			ValidationMessage = validationMessage;
			Children = children;
			Status = children.Select(c => c.Status).DefaultIfEmpty(Status.Pass).Max();
		}
	}
}
