using System;
using System.Linq;
using NSubstitute;
using Overseer.Outputs;
using Overseer.RabbitMQ;
using Overseer.Validators;
using Shouldly;
using Xunit;

namespace Overseer.Tests.Acceptance
{
	public class AcceptanceTests
	{

		private Action<object> _pushMessage;
		private InMemoryValidationOutput _output;

		public AcceptanceTests()
		{
			var reader = Substitute.For<IMessageReader>();

			reader.When(r => r.Start(Arg.Any<Action<object>>())).Do(c => _pushMessage = c.Arg<Action<object>>());

			var converter = Substitute.For<IMessageConverter>();
			var source = Substitute.For<IValidatorSource>();
			var validator = new MessageValidator(source);

			_output = new InMemoryValidationOutput();

			var queueMonitor = new MonitorQueue(reader, converter, validator, _output);
			queueMonitor.Start();
		}

		[Fact]
		public void When_reading_message_which_cannot_be_converted()
		{
			_pushMessage("testing");

			_output.Results.Single().Status.ShouldBe(Status.Warning);
			_output.Results.Single().Message.ShouldBe("Unable to convert message of type String using IMessageConverterProxy.");
		}
	}
}
