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
			var validators = _source.For(message.Type).ToList();

			if (validators.Any() == false)
			{
				return new ValidationResultLeaf(Status.Warning, string.Format("No validators for {0} have been registered.", message.Type));
			}

			var results = validators
				.Select(v => v.Validate(message))
				.ToList();

			return new ValidationResultNode(results);
		}
	}
}
