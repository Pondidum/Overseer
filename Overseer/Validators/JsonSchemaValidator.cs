using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Overseer.Validators
{
	public class JsonSchemaValidator : IValidator
	{
		public JSchema Body { get; set; }
		public string Type { get; set; }

		public ValidationResult Validate(Message message)
		{
			if (string.Equals(message.Type, Type, StringComparison.OrdinalIgnoreCase) == false)
			{
				return new ValidationResultLeaf(Status.NotInterested, string.Empty);
			}

			try
			{
				var obj = JObject.Parse(message.Body);

				IList<string> messages;

				var status = obj.IsValid(Body, out messages)
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
