using System;
using System.Linq;
using NSubstitute;
using Overseer.Converters;
using Overseer.Outputs;
using Overseer.Readers;
using Overseer.Sources;
using Overseer.Validators;
using Shouldly;
using Xunit;

namespace Overseer.Tests.Acceptance
{
	public class AcceptanceTests : IDisposable
	{
		private readonly InMemoryMessageReader _messages;
		private readonly InMemoryValidationSource _validators;
		private readonly InMemoryValidationOutput _output;
		private readonly QueueMonitor _queueMonitor;

		public AcceptanceTests()
		{
			_messages = new InMemoryMessageReader();
			_output = new InMemoryValidationOutput();
			_validators = new InMemoryValidationSource();

			var converter = new DirectMessageConverter();
			var validator = new MessageValidator(_validators);

			_queueMonitor = new QueueMonitor(_messages, converter, validator, _output);
			_queueMonitor.Start();
		}

		private ValidationResult Result { get { return _output.Results.Single(); } }

		[Fact]
		public void When_reading_message_which_cannot_be_converted()
		{
			_messages.Push("Testing");

			Result.Status.ShouldBe(Status.Warning);
			Result.Children.Single().ValidationMessage.ShouldBe("Unable to convert message of type String using DirectMessageConverter.");
		}

		[Fact]
		public void When_reading_a_message_with_no_validator()
		{
			_messages.Push(new Message
			{
				Type = "NonValidatingMessage",
			});

			Result.Status.ShouldBe(Status.Warning);
			Result.Children.Single().ValidationMessage.ShouldBe("No validators for NonValidatingMessage have been registered.");
		}

		[Fact]
		public void When_reading_a_message_which_is_not_valid()
		{
			var message = new Message
			{
				Type = "ValidatableMessage"
			};

			var validator = Substitute.For<IValidator>();
			validator.Validate(message).Returns(new ValidationNode(Status.Fail, "No."));
			_validators.Register(message.Type, validator);

			_messages.Push(message);

			Result.Status.ShouldBe(Status.Fail);
			Result.Children.Single().ValidationMessage.ShouldBe("No.");
		}

		[Fact]
		public void When_reading_a_message_which_is_valid()
		{
			var message = new Message
			{
				Type = "ValidatableMessage"
			};

			var validator = Substitute.For<IValidator>();
			validator.Validate(message).Returns(new ValidationNode(Status.Pass, string.Empty));
			_validators.Register(message.Type, validator);

			_messages.Push(message);

			Result.Status.ShouldBe(Status.Pass);
			Result.Children.Single().Status.ShouldBe(Status.Pass);
		}

		public void Dispose()
		{
			_queueMonitor.Stop();
		}
	}
}
