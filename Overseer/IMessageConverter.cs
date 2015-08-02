namespace Overseer
{
	public interface IMessageConverter
	{
		Message Convert(object input);
	}
}
