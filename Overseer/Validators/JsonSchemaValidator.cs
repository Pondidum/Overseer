using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Overseer.Validators
{
	public class JsonSchemaValidator : IValidator
	{
		public JSchema BodySchema { get; private set; }
		public string MessageType { get; private set; }

		public JsonSchemaValidator(string json)
		{
			var obj = JObject.Parse(json);

			BodySchema = JSchema.Load(obj["body"].CreateReader());
			MessageType = obj["type"].Value<string>();
		}

		public ValidationResult Validate(Message message)
		{
			if (string.Equals(message.Type, MessageType, StringComparison.OrdinalIgnoreCase) == false)
			{
				return new ValidationResultLeaf(Status.NotInterested, string.Empty);
			}

			try
			{
				var obj = JObject.Parse(message.Body);

				IList<string> messages;

				var status = obj.IsValid(BodySchema, out messages)
					? Status.Pass
					: Status.Fail;

				return new ValidationResultLeaf(status, string.Join(Environment.NewLine, messages));
			}
			catch (Exception ex)
			{
				return new ValidationResultLeaf(Status.Fail, ex.Message);
			}

		}
	}
}
