using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Overseer.Validators
{
	public class JsonSchemaValidator : IValidator
	{
		public JSchema Header { get; set; }
		public JSchema Body { get; set; }
		public string Type { get; set; }

		public ValidationNode Validate(Message message)
		{
			if (string.Equals(message.Type, Type, StringComparison.OrdinalIgnoreCase) == false)
			{
				return new ValidationNode(Status.NotInterested, string.Empty);
			}

			try
			{
				var obj = JObject.Parse(message.Body);

				IList<string> bodyMessages;
				IList<string> headerMessages;

				var bodyStatus = obj.IsValid(Body, out bodyMessages);
				var headerStatus = JObject.FromObject(message.Headers).IsValid(Header, out headerMessages);

				var status = bodyStatus && headerStatus
					? Status.Pass
					: Status.Fail;

				return new ValidationNode(status, string.Join(Environment.NewLine, headerMessages.Concat(bodyMessages)));
			}
			catch (Exception ex)
			{
				return new ValidationNode(Status.Fail, ex.Message);
			}
		}
	}
}
