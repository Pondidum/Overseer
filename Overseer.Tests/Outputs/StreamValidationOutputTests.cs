using System.Collections.Generic;
using System.IO;
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

		public StreamValidationOutputTests()
		{
			_stream = new MemoryStream();
			_output = new StreamValidationOutput(_stream);
		}

		private string GetText()
		{
			return Encoding.UTF8.GetString(_stream.ToArray()).Trim();
		}

		[Fact]
		public void When_there_are_no_children()
		{
			_output.Write(new ValidationResultLeaf(Status.Pass, "Some Message"));

			GetText()
				.ShouldBe("Pass: Some Message");
		}

		[Fact]
		public void When_there_is_a_child()
		{
			_output.Write(new ValidationResultNode(new List<ValidationResult>()
			{
				new ValidationResultLeaf(Status.NotInterested, "Another Message")
			}));

			GetText()
				.ShouldBe("NotInterested: \r\n  NotInterested: Another Message");
		}

		[Fact]
		public void When_there_are_multiple_children()
		{
			_output.Write(new ValidationResultNode(new List<ValidationResult>()
			{
				new ValidationResultNode(new List<ValidationResult>()
				{
					new ValidationResultLeaf(Status.Fail, "Omg!")
				})
			}));

			GetText()
				.ShouldBe("Fail: \r\n  Fail: \r\n    Fail: Omg!");
		}
	}
}
