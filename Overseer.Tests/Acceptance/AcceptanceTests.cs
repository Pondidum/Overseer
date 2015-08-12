using System;
using System.Linq;
using NSubstitute;
using Overseer.Outputs;
using Overseer.RabbitMQ;
using Overseer.Readers;
using Overseer.Validators;
using Shouldly;
using Xunit;

namespace Overseer.Tests.Acceptance
{
	public class AcceptanceTests
	{
		private readonly InMemoryMessageReader _messages;
		private readonly InMemoryValidationOutput _output;

		public AcceptanceTests()
		{
			_messages = new InMemoryMessageReader();
			_output = new InMemoryValidationOutput();

			var converter = Substitute.For<IMessageConverter>();
			var source = Substitute.For<IValidatorSource>();
			var validator = new MessageValidator(source);

			var queueMonitor = new MonitorQueue(_messages, converter, validator, _output);
			queueMonitor.Start();
		}

		[Fact]
		public void When_reading_message_which_cannot_be_converted()
		{
			_messages.Push("Testing");

			_output.Results.Single().Status.ShouldBe(Status.Warning);
			_output.Results.Single().Message.ShouldBe("Unable to convert message of type String using IMessageConverterProxy.");
		}
	}
}
