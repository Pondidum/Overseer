using System.Linq;
using Overseer.Sources;
using Shouldly;
using Xunit;

namespace Overseer.Tests.Sources
{
	public class ResourceValidatorSourceTests
	{
		private readonly string _pattern;

		public ResourceValidatorSourceTests()
		{
			_pattern = "Overseer.Tests.Resources." + ResourceValidatorSource.MessageTypeReplacementTag + ".json";
		}

		[Fact]
		public void When_reading_a_valid_resource()
		{
			var source = new ResourceValidatorSource(GetType().Assembly, _pattern);
			source.For("PersonExactMatch").ShouldNotBeEmpty();
		}

		[Fact]
		public void When_reading_a_multi_source_resource()
		{
			var source = new ResourceValidatorSource(GetType().Assembly, "Overseer.Tests.Resources.multispec.json");
			source.For("PersonExactMatch").Count().ShouldBe(1);
		}

		[Fact]
		public void When_reading_an_invalid_resouce()
		{
			var source = new ResourceValidatorSource(GetType().Assembly, _pattern);
			source.For("wefwefwf").ShouldBeEmpty();
		}
	}
}
