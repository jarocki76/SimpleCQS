using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using NUnit.Framework;
using SimpleCQS.Command.Validation;

namespace SimpleCQS.Tests.Command.Validation
{
  [TestFixture]
  public class ValidationProcessorTests
  {
    [Test]
    public void Validate_ValidatorListIsEmpty_ThrowProperlyValidatorIsNotRegisteredException()
    {
      var validators = new List<IValidator>();
      var validationProcessor = new ValidationProcessor(validators);

      Assert.That(() => validationProcessor.Validate(new object()), 
        Throws.InstanceOf<ProperlyValidatorIsNotRegisteredException>()
        .And.Message.EqualTo(string.Format("Properly validator is not registered for type {0}", typeof(object).Name)));
    }

    [Test]
    public void Validate_ValidatorIsNotRegistered_ThrowProperlyValidatorIsNotRegisteredException()
    {
      var instance = new object();
      var validators = new List<IValidator> { new StringValidator() };
      var validationProcessor = new ValidationProcessor(validators);

      Assert.That(() => validationProcessor.Validate(instance),
        Throws.InstanceOf<ProperlyValidatorIsNotRegisteredException>()
        .And.Message.EqualTo(string.Format("Properly validator is not registered for type {0}", instance.GetType().Name)));
    }

    [Test]
    public void Validate_ValidatorIsRegistered_ProperlyValidatorValidateMustHasHappend()
    {
      var instance = new object();
      var objectValidator = new ObjectValidator(new ValidationResult());
      var validators = new List<IValidator> { new StringValidator(), objectValidator };
      var validationProcessor = new ValidationProcessor(validators);

      validationProcessor.Validate(instance);

      Assert.IsTrue((objectValidator).ValidateHasHappend);
    }

    [Test]
    public void Validate_ValidatorIsRegistered_NotProperlyValidatorValidateMustNotHasHappend()
    {
      var instance = new object();
      var validators = new List<IValidator> {new StringValidator(), new ObjectValidator(new ValidationResult())};

      var validationProcessor = new ValidationProcessor(validators);

      validationProcessor.Validate(instance);

      Assert.IsFalse((new StringValidator()).ValidateHasHappend);
    }

    [Test]
    public void Validate_ValidatorIsRegisteredAndInstanceIsValid_ReturValidValidationStatus()
    {
      var instance = new object();
      var validators = new List<IValidator> {new StringValidator(), new ObjectValidator(new ValidationResult())};
      var validationProcessor = new ValidationProcessor(validators);

      var validationStatus = validationProcessor.Validate(instance);

      Assert.IsTrue(validationStatus.IsValid);
    }

    [Test]
    public void Validate_ValidatorIsRegisteredAndInstanceIsNotValid_ReturnInvalidValidationStatus()
    {
      var instance = new object();
      var objectValidator = new ObjectValidator(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("prop", "error") }));
      var validators = new List<IValidator> {new StringValidator(), objectValidator};
      var validationProcessor = new ValidationProcessor(validators);

      var validationStatus = validationProcessor.Validate(instance);

      Assert.IsFalse(validationStatus.IsValid);
    }

    [Test]
    public void Validate_ValidatorIsRegisteredAndInstanceIsNotValid_ReturnProperlyErrorMesssage()
    {
      var instance = new object();
      var validationResult = new ValidationResult(new List<ValidationFailure> {new ValidationFailure("prop", "error")});
      var objectValidator = new ObjectValidator(validationResult);
      var validators = new List<IValidator> { new StringValidator(), objectValidator };
      var validationProcessor = new ValidationProcessor(validators);

      var validationError = validationProcessor.Validate(instance).ValidationErrors.SingleOrDefault(e => true);
      
      Assert.AreEqual(string.Format("[ValidationError][Property='{0}'][AttemptedValue='{1}'][Message='{2}']",
        validationResult.Errors[0].PropertyName,
        validationResult.Errors[0].AttemptedValue,
        validationResult.Errors[0].ErrorMessage), 
        validationError.Message);
    }

    internal class ObjectValidator : AbstractValidator<object>
    {
      private readonly ValidationResult _validationResult;
      public bool ValidateHasHappend { get; private set; }

      public ObjectValidator(ValidationResult validationResult)
      {
        _validationResult = validationResult;
      }

      public override ValidationResult Validate(object instance)
      {
        ValidateHasHappend = true;

        return _validationResult;
      }
    }

    internal class StringValidator : AbstractValidator<string>
    {
      public bool ValidateHasHappend { get; private set; }

      public override ValidationResult Validate(string instance)
      {
        ValidateHasHappend = true;

        return new ValidationResult();
      }
    }
  }
}