using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.ContentTypes;
using Overseer.Sources;
using Overseer.Validators;
using Owin;
using Shouldly;
using Xunit;

namespace Overseer.Tests.Sources
{
	public class UrlValidatorSourceTests : IDisposable
	{
		private readonly IDisposable _host;
		private readonly string _baseUrl;

		public UrlValidatorSourceTests()
		{
			_baseUrl = "http://localhost:45354/";

			_host = WebApp.Start(_baseUrl, app =>
			{
				app.UseFileServer(new FileServerOptions
				{
					EnableDirectoryBrowsing = true,
					FileSystem = new EmbeddedResourceFileSystem(),
					StaticFileOptions =
					{
						ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
						{
							{ ".json", "application/json" }
						})
					}
				});
			});
		}

		[Fact]
		public void The_server_should_work()
		{
			var client = new HttpClient();
			var result = client.GetAsync(_baseUrl + "Overseer.Tests.Resources.PersonExactMatch.json").Result;

			result.StatusCode.ShouldBe(HttpStatusCode.OK);
		}

		[Fact]
		public void When_getting_a_validator_which_exists()
		{
			var source = new UrlValidatorSource(_baseUrl + "Overseer.Tests.Resources.{messageType}.json");

			source.For("PersonExactMatch").Single().ShouldBeOfType<JsonSchemaValidator>();
		}

		[Fact]
		public void When_getting_a_validator_which_doesnt_exist()
		{
			var source = new UrlValidatorSource(_baseUrl + "Overseer.Tests.Resources.{messageType}.json");

			source.For("Asdafewweg").ShouldBeEmpty();
		}

		[Fact]
		public void When_getting_from_a_source_with_multiple_validators()
		{
			var source = new UrlValidatorSource(_baseUrl + "Overseer.Tests.Resources.multispec.json");

			source.For("PersonExactMatch").Cast<JsonSchemaValidator>().Single().Type.ShouldBe("PersonExactMatch");
			source.For("PersonOtherMatch").Cast<JsonSchemaValidator>().Single().Type.ShouldBe("PersonOtherMatch");
		}

		public void Dispose()
		{
			_host.Dispose();
		}
	}
}