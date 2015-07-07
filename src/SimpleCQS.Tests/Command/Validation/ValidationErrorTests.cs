using NUnit.Framework;
using SimpleCQS.Command.Validation;

namespace SimpleCQS.Tests.Command.Validation
{
  [TestFixture]
  public class ValidationErrorTests
  {
    [Test]
    public void Constructor_SetMessage()
    {
      const string message = "Error message";
      
      var validationError = new ValidationError(message);

      Assert.AreEqual(message, validationError.Message);
    }
  }
}