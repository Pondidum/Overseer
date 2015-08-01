namespace Overseer
{
	public interface IMessageValidator
	{
		ValidationResult Validate(object message);
	}
}
