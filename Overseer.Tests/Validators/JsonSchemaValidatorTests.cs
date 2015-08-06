using System;
using System.IO;
using Newtonsoft.Json;
using Overseer.Tests.Resources;
using Overseer.Validators;
using Shouldly;
using Xunit;

namespace Overseer.Tests.Validators
{
	public class JsonSchemaValidatorTests
	{
		private readonly JsonSchemaValidator _validator;

		public JsonSchemaValidatorTests()
		{
			using (var stream = GetType().Assembly.GetManifestResourceStream("Overseer.Tests.Resources.spec.json"))
			using (var reader = new StreamReader(stream))
			{
				_validator = new JsonSchemaValidator(reader.ReadToEnd());
			}
		}

		[Fact]
		public void When_the_message_is_blank()
		{
			_validator
				.Validate(new Message())
				.Status
				.ShouldBe(Status.NotInterested);
		}

		[Fact]
		public void When_the_message_is_for_a_different_type()
		{
			_validator
				.Validate(new Message { Type = "SomeType" })
				.Status
				.ShouldBe(Status.NotInterested);
		}

		[Fact]
		public void When_the_message_is_missing_data()
		{
			var body = JsonConvert.SerializeObject(new PersonExactMatch());
			var result = _validator.Validate(new Message { Type = "PersonExactMatch", Body = body });

			result.Status.ShouldBe(Status.Fail);
			result.Message.ShouldNotBeEmpty();
		}

		[Fact]
		public void When_the_message_matches()
		{
			var body = JsonConvert.SerializeObject(new PersonExactMatch
			{
				ID = Guid.NewGuid(), 
				Name = "Testing Person", 
				Addresses = new[] { new Address { Line1 = "150", PostCode = "RG1 5JN"}, }
			});
			
			var result = _validator.Validate(new Message { Type = "PersonExactMatch", Body = body });

			result.Message.ShouldBeEmpty();
			result.Status.ShouldBe(Status.Pass);
		}
	}
}
