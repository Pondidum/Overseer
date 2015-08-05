namespace Overseer
{
	public interface IValidator
	{
		ValidationResult Validate(Message message);
	}
}
