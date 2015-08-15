using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Overseer.Sources;
using Overseer.Validators;
using Shouldly;
using Xunit;

namespace Overseer.Tests.Sources
{
	public class FileValidatorSourceTests : IDisposable
	{
		private readonly string _directory;
		private FileValidatorSource _source;

		public FileValidatorSourceTests()
		{
			_directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			var pattern = Path.Combine(_directory, FileValidatorSource.MessageTypeReplacementTag + ".json");

			Directory.CreateDirectory(_directory);
			_source = new FileValidatorSource(pattern);
		}

		[Fact]
		public void When_the_directory_doesnt_exist()
		{
			Directory.Delete(_directory);
			_source.For("SomeTest").ShouldBeEmpty();
		}

		[Fact]
		public void When_the_file_doesnt_exist()
		{
			_source.For("SomeTest").ShouldBeEmpty();
		}

		[Fact]
		public void When_the_file_exists_but_is_empty()
		{
			File.Create(Path.Combine(_directory, "PersonExactMatch.json"));

			_source.For("PersonExactMatch").ShouldBeEmpty();
		}

		[Fact]
		public void When_the_file_exists_and_contains_a_single_spec()
		{
			File.WriteAllText(Path.Combine(_directory, "PersonExactMatch.json"), GetSpecJson());

			_source.For("PersonExactMatch").Single().ShouldBeOfType<JsonSchemaValidator>();
		}

		[Fact]
		public void When_the_file_exists_and_contains_an_array_of_specs()
		{
			var spec = GetSpecJson();
			var contents = string.Format("[{0}, {1}]", spec, spec);

			File.WriteAllText(Path.Combine(_directory, "PersonExactMatch.json"), contents);

			var validators = _source.For("PersonExactMatch").ToList();
			validators[0].ShouldBeOfType<JsonSchemaValidator>();
			validators[1].ShouldBeOfType<JsonSchemaValidator>();
		}

		[Fact]
		public void When_the_file_contains_specs_for_different_messages()
		{
			File.WriteAllText(Path.Combine(_directory, "PersonOtherMatch.json"), GetMultiSpecJson());

			var validators = _source.For("PersonOtherMatch").ToList();
			validators.Single().ShouldBeOfType<JsonSchemaValidator>();
		}

		private string GetSpecJson()
		{
			using (var stream = GetType().Assembly.GetManifestResourceStream("Overseer.Tests.Resources.PersonExactMatch.json"))
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
		
		private string GetMultiSpecJson()
		{
			using (var stream = GetType().Assembly.GetManifestResourceStream("Overseer.Tests.Resources.multispec.json"))
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		public void Dispose()
		{
			try
			{
				if (Directory.Exists(_directory))
				{
					Directory.Delete(_directory, true);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
