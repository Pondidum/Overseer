using System;

namespace Overseer.Readers
{
	public class InMemoryMessageReader : IMessageReader
	{
		private Action<object> _pushMessage;

		public InMemoryMessageReader()
		{
			Stop();
		}

		public void Push(object message)
		{
			_pushMessage(message);
		}

		public void Start(Action<object> onMessage)
		{
			_pushMessage = onMessage;
		}

		public void Stop()
		{
			_pushMessage = message => { };
		}
	}
}
