using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Overseer.Outputs;
using Shouldly;
using Xunit;

namespace Overseer.Tests.Outputs
{
	public class StreamValidationOutputTests
	{
		private readonly MemoryStream _stream;
		private readonly StreamValidationOutput _output;
		private readonly Message _message;

		public StreamValidationOutputTests()
		{
			_stream = new MemoryStream();
			_output = new StreamValidationOutput(_stream);
			_message = new Message { Type = "CandidateCreatedEvent" };
		}

		private string GetText()
		{
			return Encoding.UTF8.GetString(_stream.ToArray()).Trim();
		}

		[Fact]
		public void When_there_are_no_children()
		{
			_output.Write(new ValidationResult(_message, Enumerable.Empty<ValidationNode>()));

			GetText()
				.ShouldBe("* [Pass] " + _message.Type);
		}

		[Fact]
		public void When_there_is_a_child()
		{
			_output.Write(new ValidationResult(_message, new[]
			{
				new ValidationNode(Status.NotInterested, "Another Message")
			}));

			GetText()
				.ShouldBe("* [NotInterested] " + _message.Type + "\r\n  * [NotInterested]: Another Message");
		}

		[Fact]
		public void When_there_are_multiple_children()
		{
			_output.Write(new ValidationResult(_message, new[]
			{
				new ValidationNode("For some reason", new []
				{
					new ValidationNode(Status.Fail, "Omg!")
				})
			}));

			GetText()
				.ShouldBe("* [Fail] " + _message.Type + "\r\n  * [Fail]: For some reason\r\n    * [Fail]: Omg!");
		}
	}
}
