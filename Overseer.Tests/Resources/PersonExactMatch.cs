using System;
using System.Collections.Generic;

namespace Overseer.Tests.Resources
{
	public class PersonExactMatch
	{
		public Guid ID { get; set; }
		public string Name { get; set; }

		public IEnumerable<Address> Addresses { get; set; } 
	}
}
