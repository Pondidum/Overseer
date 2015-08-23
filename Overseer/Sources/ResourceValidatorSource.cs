using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Overseer.Validators;

namespace Overseer.Sources
{
	public class ResourceValidatorSource : IValidatorSource
	{
		public const string MessageTypeReplacementTag = "{messageType}";

		private readonly Assembly _assembly;
		private readonly string _filePattern;

		public ResourceValidatorSource(Assembly assembly, string filePattern)
		{
			_assembly = assembly;
			_filePattern = filePattern;
		}

		private string GetFilename(string messageType)
		{
			return _filePattern.Replace(MessageTypeReplacementTag, messageType);
		}

		public IEnumerable<IValidator> For(string messageType)
		{
			var fullname = GetFilename(messageType);

			try
			{
				using (var stream = _assembly.GetManifestResourceStream(fullname))
				{
					return JsonSchemaReader
						.FromJsonStream(stream)
						.Where(v => string.Equals(v.Type, messageType)); ;
				}
			}
			catch (Exception)
			{
				return Enumerable.Empty<IValidator>();
			}
		}
	}
}
