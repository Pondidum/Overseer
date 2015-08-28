namespace Overseer
{
	public interface IValidator
	{
		ValidationNode Validate(Message message);
	}
}
