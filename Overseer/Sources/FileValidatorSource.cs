using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Overseer.Validators;

namespace Overseer.Sources
{
	public class FileValidatorSource : IValidatorSource
	{
		public const string MessageTypeReplacementTag = "{messageType}";

		private readonly string _filePattern;

		public FileValidatorSource(string filePattern)
		{
			_filePattern = filePattern;
		}

		private string GetFilename(string messageType)
		{
			return _filePattern.Replace(MessageTypeReplacementTag, messageType);
		}

		public IEnumerable<IValidator> For(string messageType)
		{
			var path = Path.Combine(_filePattern, GetFilename(messageType));

			try
			{
				using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					return JsonSchemaReader
						.FromJsonStream(stream)
						.Where(v => string.Equals(v.Type, messageType)); ;
				}
			}
			catch (Exception)
			{
				return Enumerable.Empty<IValidator>();
				//throw;
			}
		}
	}
}
