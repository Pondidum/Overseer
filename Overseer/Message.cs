using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Overseer
{
	public class Message
	{
		public Dictionary<string, object> Headers { get; set; }
		public object Body { get; set; }

		public Message()
		{
			Headers = new Dictionary<string, object>();
		}

	}
}
