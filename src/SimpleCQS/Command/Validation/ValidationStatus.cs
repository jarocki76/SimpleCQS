using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCQS.Command.Validation
{
  public class ValidationStatus : IValidationStatus
  {
    public List<ValidationError> ValidationErrors { get; private set; }

    public bool IsValid
    {
      get { return !ValidationErrors.Any(); }
    }

    public ValidationStatus()
    {
      ValidationErrors = new List<ValidationError>();
    }

    public ValidationStatus(List<ValidationError> validationErrors)
    {
      if (validationErrors == null)
      {
        throw new ArgumentNullException("validationErrors", "Can not be null");
      }
      
      ValidationErrors = validationErrors;
    }

    public void AddValidationError(ValidationError validationError)
    {
      if (validationError == null)
      {
        throw new ArgumentNullException("validationError", "Can not be null");
      }
      
      ValidationErrors.Add(validationError);
    }
  }
}