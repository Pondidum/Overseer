using System.Linq;

namespace Overseer
{
	public class MessageValidator
	{
		private readonly IValidatorSource _source;

		public MessageValidator(IValidatorSource source)
		{
			_source = source;
		}

		public ValidationResult Validate(Message message)
		{
			var validators = _source.For(message.Type);

			var results = validators
				.Select(v => v.Validate(message));

			return new ValidationResult(results);
		}
	}
}
