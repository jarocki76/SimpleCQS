using System;

namespace SimpleCQS.Command.Validation
{
  public class ValidationException : Exception
  {
    public IValidationStatus ValidationValidationStatus { get; private set; }

    public ValidationException(string message, IValidationStatus validationStatus)
      : base(message)
    {
      ValidationValidationStatus = validationStatus;
    }

  }
}