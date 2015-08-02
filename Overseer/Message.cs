using System.Collections.Generic;

namespace Overseer
{
	public class Message
	{
		public IDictionary<string, object> Headers { get; set; }
		public string Body { get; set; }

		public Message()
		{
			Headers = new Dictionary<string, object>();
		}

	}
}
