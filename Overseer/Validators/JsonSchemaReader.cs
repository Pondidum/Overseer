using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Overseer.Validators
{
	public class JsonSchemaReader
	{
		public static IEnumerable<JsonSchemaValidator> FromJsonStream(Stream stream)
		{
			using (var reader = new StreamReader(stream))
			{
				var json = reader.ReadToEnd();
				var token = JToken.Parse(json);

				var validators = token.Type == JTokenType.Array
					? token.ToObject<IEnumerable<JsonSchemaValidator>>()
					: new[] {token.ToObject<JsonSchemaValidator>()};

				return validators;
			}
		}
	}
}
