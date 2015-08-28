namespace Overseer
{
	public class QueueMonitor
	{
		private readonly IMessageReader _reader;
		private readonly IMessageConverter _converter;
		private readonly MessageValidator _validator;
		private readonly IValidationOutput _output;

		public QueueMonitor(IMessageReader reader, IMessageConverter converter, MessageValidator validator, IValidationOutput output)
		{
			_reader = reader;
			_converter = converter;
			_validator = validator;
			_output = output;
		}

		public void Start()
		{
			_reader.Start(OnMessage);
		}

		public void Stop()
		{
			_reader.Stop();
		}

		private void OnMessage(object input)
		{
			var message = _converter.Convert(input);

			var result = message != null
				? _validator.Validate(message) 
				: BadMessageResult(input);

			_output.Write(result);
		}

		private ValidationResult BadMessageResult(object input)
		{
			var message = string.Format(
				"Unable to convert message of type {0} using {1}.",
				input.GetType().Name,
				_converter.GetType().Name);

			return new ValidationResult(null, new []
			{
				new ValidationNode(Status.Warning, message) 
			});
		}
	}
}
