namespace SimpleCQS.Command.Validation
{
  public class ValidationError
  {
    public ValidationError(string message)
    {
      Message = message;
    }

    public object Message { get; private set; }
  }
}