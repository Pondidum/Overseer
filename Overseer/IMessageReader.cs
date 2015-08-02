using System;

namespace Overseer
{
	public interface IMessageReader
	{
		void Start(Action<object> onMessage);
		void Stop();
	}
}
