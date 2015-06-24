using NUnit.Framework;
using SimpleCQS.Command.Validation;

namespace SimpleCQS.Tests.Command.Validation
{
  [TestFixture]
  public class ValidationExecptionTests
  {
    [Test]
    public void ThrowNewValidationException_SetValidationStatus()
    {
      IValidationStatus validationStatus = new ValidationStatus();
      try
      {
        throw new ValidationException("Error", validationStatus);
      }
      catch (ValidationException ex)
      {
        Assert.AreSame(validationStatus, ex.ValidationValidationStatus); 
      }
    }
  }
}