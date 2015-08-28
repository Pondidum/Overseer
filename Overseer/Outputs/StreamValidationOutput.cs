using System.Collections.Generic;
using System.IO;

namespace Overseer.Outputs
{
	public class StreamValidationOutput : IValidationOutput
	{
		private readonly StreamWriter _writer;

		public StreamValidationOutput(Stream stream)
		{
			_writer = new StreamWriter(stream)
			{
				AutoFlush = true
			};
		}

		public void Write(ValidationResult result)
		{
			_writer.WriteLine($"* [{result.Status}] {result.Message.Type}");

			WriteRecursive(result.Children, 1);
		}

		private void WriteRecursive(IEnumerable<ValidationNode> nodes, int depth)
		{
			foreach (var node in nodes)
			{
				var offset = new string(' ', depth * 2);

				_writer.WriteLine($"{offset}* [{node.Status}]: {node.ValidationMessage}");

				WriteRecursive(node.Children, depth + 1);
			}
		}
	}
}
