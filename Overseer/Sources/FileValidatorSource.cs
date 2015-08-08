using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Overseer.Validators;

namespace Overseer.Sources
{
	public class FileValidatorSource : IValidatorSource
	{
		private readonly string _baseDirectory;

		public FileValidatorSource(string baseDirectory)
		{
			_baseDirectory = baseDirectory;
		}

		public IEnumerable<IValidator> For(string messageType)
		{
			var filename = messageType + ".json";
			var path = Path.Combine(_baseDirectory, filename);

			try
			{
				using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				using (var reader = new StreamReader(fs))
				{
					var json = reader.ReadToEnd();
					var token = JToken.Parse(json);

					return token.Type == JTokenType.Array 
						? token.ToObject<IEnumerable<JsonSchemaValidator>>() 
						: new[] { token.ToObject<JsonSchemaValidator>() };
				}
			}
			catch (Exception ex)
			{
				return Enumerable.Empty<IValidator>();
				//throw;
			}
		}
	}
}
