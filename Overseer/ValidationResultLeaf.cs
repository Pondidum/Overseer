namespace Overseer
{
	public class ValidationResultLeaf : ValidationResult
	{
		public ValidationResultLeaf(Status status, string message)
		{
			Status = status;
			Message = message;
		}
	}
}
