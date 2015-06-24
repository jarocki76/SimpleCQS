using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleCQS.Command.Validation;

namespace SimpleCQS.Tests.Command.Validation
{
  [TestFixture]
  public class ValidationStatusTests
  {
    [Test]
    public void Constructor_SetEmptyValidationErrors()
    {
      var validationState = new ValidationStatus();

      Assert.IsEmpty(validationState.ValidationErrors.ToList());
    }

    [Test]
    public void ConstructorWithValidationErrors_ValidationErrorsIsNotNull_SetValidationErrors()
    {
      var validationErrors = new List<ValidationError>();
      
      var validationState = new ValidationStatus(validationErrors);

      Assert.AreSame(validationErrors, validationState.ValidationErrors);
    }

    [Test]
    public void ConstructorWithValidationErrors_ValidationErrorsIsNull_ThrowArgumentNullException()
    {
      List<ValidationError> validationErrors = null;

      Assert.Throws<ArgumentNullException>(() =>  new ValidationStatus(validationErrors));
    }

    [Test]
    public void AddValidationError_AddValidationError_OneValidationErrorInValidetionErrors()
    {
      var validationError = new ValidationError("message");
      var validationState = new ValidationStatus();

      validationState.AddValidationError(validationError);
      
      Assert.AreEqual(1, validationState.ValidationErrors.ToList().Count);
    }

    [Test]
    public void AddValidationError_AddNull_ThrowArgumentNullException()
    {
      ValidationError validationError = null;
      var validationState = new ValidationStatus();

      Assert.Throws<ArgumentNullException>(() => validationState.AddValidationError(validationError));
    }

    [Test]
    public void IsValid_ValidErrorsIsEmpty_ReturnTrue()
    {
      var validationState = new ValidationStatus();

      var result = validationState.IsValid;

      Assert.IsTrue(result);
    }

    [Test]
    public void IsValid_ValidErrorsIsNot_ReturnFalse()
    {
      var validationState = new ValidationStatus();
      validationState.AddValidationError(new ValidationError("message"));

      var result = validationState.IsValid;

      Assert.IsFalse(result);
    }
  }
}