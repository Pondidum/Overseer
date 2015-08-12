namespace Overseer.Converters
{
	public class DirectMessageConverter : IMessageConverter
	{
		public Message Convert(object input)
		{
			return input as Message;
		}
	}
}
