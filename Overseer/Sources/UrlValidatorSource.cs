using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Overseer.Validators;

namespace Overseer.Sources
{
	public class UrlValidatorSource : IValidatorSource
	{
		private readonly string _urlPattern;

		public UrlValidatorSource(string urlPattern)
		{
			_urlPattern = urlPattern;
		}

		public IEnumerable<IValidator> For(string messageType)
		{
			var url = _urlPattern.Replace("{messageType}", messageType);

			using (var client = new HttpClient())
			{
				var response = client.GetAsync(url).Result;

				if (response.IsSuccessStatusCode == false)
				{
					return Enumerable.Empty<IValidator>();
				}

				var json = response.Content.ReadAsStringAsync().Result;
				var token = JToken.Parse(json);

				var validators = token.Type == JTokenType.Array
					? token.ToObject<IEnumerable<JsonSchemaValidator>>()
					: new[] { token.ToObject<JsonSchemaValidator>() };

				return validators
					.Where(v => string.Equals(v.Type, messageType));
			}
		}
	}
}
