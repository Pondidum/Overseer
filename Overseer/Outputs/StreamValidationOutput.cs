using System.IO;
using System.Linq;

namespace Overseer.Outputs
{
	public class StreamValidationOutput : IValidationOutput
	{
		private readonly StreamWriter _writer;

		public StreamValidationOutput(Stream stream)
		{
			_writer = new StreamWriter(stream);
			_writer.AutoFlush = true;
		}

		public void Write(ValidationResult result)
		{
			WriteRecursive(result, 0);
		}

		private void WriteRecursive(ValidationResult result, int depth)
		{
			_writer.Write(new string(' ', depth * 2));
			_writer.Write(result.Status);
			_writer.Write(": ");

			if (result.Results.Any())
			{
				_writer.WriteLine();

				foreach (var child in result.Results)
				{
					WriteRecursive(child, depth + 1);
				}
			}
			else
			{
				_writer.Write(result.ValidationMessage);
			}

			_writer.WriteLine();
		}
	}
}
