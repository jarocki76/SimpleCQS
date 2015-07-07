using System.Collections.Generic;

namespace SimpleCQS.Command.Validation
{
  public interface IValidationStatus
  {
    List<ValidationError> ValidationErrors { get; }

    bool IsValid { get; }

    void AddValidationError(ValidationError validationError);
  }
}