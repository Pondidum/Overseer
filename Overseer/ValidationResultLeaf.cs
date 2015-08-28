namespace Overseer
{
	public class ValidationResultLeaf : ValidationResult
	{
		public ValidationResultLeaf(Status status, string validationMessage)
		{
			Status = status;
			ValidationMessage = validationMessage;
		}
	}
}
