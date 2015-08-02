namespace Overseer
{
	public class ValidationResult
	{
		public string Message { get; private set; }
		public Status Status { get; private set; }

		public ValidationResult(Status status, string message)
		{
			Status = status;
			Message = message;
		}
	}
}
