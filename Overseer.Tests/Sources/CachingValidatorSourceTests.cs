using System;
using System.Linq;
using NSubstitute;
using Overseer.Sources;
using Shouldly;
using Xunit;

namespace Overseer.Tests.Sources
{
	public class CachingValidatorSourceTests
	{
		private readonly IValidatorSource _other;
		private readonly CachingValidatorSource _cached;

		public CachingValidatorSourceTests()
		{
			_other = Substitute.For<IValidatorSource>();
			_cached = new CachingValidatorSource(_other);
		}
		[Fact]
		public void When_the_other_source_is_null()
		{
			Should.Throw<ArgumentNullException>(() => new CachingValidatorSource(null));
		}

		[Fact]
		public void When_fetching_a_validator()
		{
			var validator = Substitute.For<IValidator>();

			_other.For(Arg.Any<string>()).Returns(new[] { validator });

			_cached.For("test").ShouldBe(new[] { validator });
			_other.Received(1).For("test");
		}

		[Fact]
		public void When_fetching_a_validator_multiple_times()
		{
			_other.For(Arg.Any<string>()).Returns(Enumerable.Empty<IValidator>());

			_cached.For("test").ShouldBeEmpty();
			_cached.For("test").ShouldBeEmpty();

			_other.Received(1).For("test");
		}
	}
}
