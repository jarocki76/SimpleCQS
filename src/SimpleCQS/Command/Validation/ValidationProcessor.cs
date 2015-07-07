using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace SimpleCQS.Command.Validation
{
  public class ValidationProcessor : IValidationProcessor
  {
    private readonly IEnumerable<IValidator> _validators;

    public ValidationProcessor(IEnumerable<IValidator> validators)
    {
      _validators = validators;
    }

    public IValidationStatus Validate<T>(T instance)
    {
      var validator = FindValidator<T>();
      if (validator == null)
      {
        var errorMessage = string.Format("Properly validator is not registered for type {0}", typeof(T).Name);
        throw new ProperlyValidatorIsNotRegisteredException(errorMessage);
      }

      ValidationResult result = validator.Validate(instance);
      IValidationStatus validationStatus = BuildValidationStatus<T>(result);
      
      return validationStatus;
    }

    private IValidator FindValidator<T>()
    {
      var validatorName = string.Format("{0}Validator", typeof(T).Name);
      IValidator validator = _validators.SingleOrDefault(v => string.Equals(v.GetType().Name, validatorName, StringComparison.CurrentCultureIgnoreCase));
      return validator;
    }

    private IValidationStatus BuildValidationStatus<T>(ValidationResult result)
    {
      IValidationStatus validationStatus = new ValidationStatus();
      if (!result.IsValid)
      {
        const string logFormat = "[ValidationError][Property='{0}'][AttemptedValue='{1}'][Message='{2}']";
        foreach (var error in result.Errors)
        {
          var errorMessage = string.Format(logFormat, error.PropertyName, error.AttemptedValue, error.ErrorMessage);
          validationStatus.AddValidationError(new ValidationError(errorMessage));
        }
      }
      return validationStatus;
    }
  }
}