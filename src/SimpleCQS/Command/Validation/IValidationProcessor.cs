namespace SimpleCQS.Command.Validation
{
  public interface IValidationProcessor
  {
    IValidationStatus Validate<T>(T objectToValidation);
  }
}