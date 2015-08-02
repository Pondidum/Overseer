namespace Overseer
{
	public class MonitorQueue
	{
		private readonly IMessageReader _reader;
		private readonly IMessageConverter _converter;
		private readonly MessageValidator _validator;
		private readonly IValidationOutput _output;

		public MonitorQueue(IMessageReader reader, IMessageConverter converter, MessageValidator validator, IValidationOutput output)
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
			var result = _validator.Validate(message);

			_output.Write(result);
		}
	}
}
