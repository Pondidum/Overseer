using System.Collections.Generic;
using System.Linq;

namespace Overseer
{
	public class ValidationResult
	{
		public Message Message { get; protected set; }
		public Status Status { get; protected set; }
		public IEnumerable<ValidationNode> Children { get; protected set; }

		public ValidationResult(Message message, IEnumerable<ValidationNode> nodes)
		{
			var children = nodes.ToList();

			Message = message ?? new Message();
			Children = children;
			Status = children.Select(c => c.Status).DefaultIfEmpty(Status.Pass).Max();
		}
	}

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
