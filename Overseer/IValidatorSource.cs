using System.Collections.Generic;

namespace Overseer
{
	public interface IValidatorSource
	{
		IEnumerable<IValidator> For(string messageType);
	}
}
