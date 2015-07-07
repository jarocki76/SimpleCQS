using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using SimpleCQS.Command;
using SimpleCQS.Command.Validation;

namespace SimpleCQS.Tests.Command
{
  [TestFixture]
  public class ValidatedCommandDispatcherTests
  {
    private ICommand _command;
    private ICommandDispatcher _commandDispatcher;
    private IValidationProcessor _validationProcessor;

    private ValidatedCommandDispatcher _validatedCommandDispatcher;

    [SetUp]
    public void SetUp()
    {
      _command = A.Fake<ICommand>();
      _commandDispatcher = A.Fake<ICommandDispatcher>();
      _validationProcessor = A.Fake<IValidationProcessor>();
      _validatedCommandDispatcher = new ValidatedCommandDispatcher(_commandDispatcher, _validationProcessor);
    }

    [Test]
    public void Dispatch_ValidateInvoiceCommand()
    {
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus());

      _validatedCommandDispatcher.Dispatch(_command);

      A.CallTo(() => _validationProcessor.Validate(_command)).MustHaveHappened();
    }

    [Test]
    public void Dispatch_InvoiceModelIsInvalid_ThrowValidationException()
    {
      var validationStatus = new ValidationStatus(new List<ValidationError> {new ValidationError("error")});
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(validationStatus);

      Assert.That(() => _validatedCommandDispatcher.Dispatch(_command), 
        Throws.InstanceOf<ValidationException>()
        .And.Message.EqualTo(string.Format("Command {0} is incorrect", typeof(ICommand).Name))
        .And.Property("ValidationValidationStatus").SameAs(validationStatus));
    }

    [Test]
    public void Dispatch_InvoiceModelIsInvalid_CommandDispatcherDispatchMustNotHaveHappened()
    {
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus(new List<ValidationError> { new ValidationError("error") }));

      Assert.Throws<ValidationException>(() => _validatedCommandDispatcher.Dispatch(_command));

      A.CallTo(() => _commandDispatcher.Dispatch(A<ICommand>._)).MustNotHaveHappened();
    }

    [Test]
    public void Dispatch_InvoiceModelIsValid__CommandDispatcherDispatchMustHaveHappened()
    {
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus());

      _validatedCommandDispatcher.Dispatch(_command);

      A.CallTo(() => _commandDispatcher.Dispatch(_command)).MustHaveHappened();
    }

    [Test]
    public void DispatchAsync_ValidateInvoiceCommand()
    {
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus());

      _validatedCommandDispatcher.DispatchAsync(_command);

      A.CallTo(() => _validationProcessor.Validate(_command)).MustHaveHappened();
    }

    [Test]
    public void DispatchAsync_InvoiceModelIsInvalid_ThrowValidationException()
    {
      var validationStatus = new ValidationStatus(new List<ValidationError> { new ValidationError("error") });
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(validationStatus);

      Assert.That(() => _validatedCommandDispatcher.DispatchAsync(_command),
        Throws.InstanceOf<ValidationException>()
        .And.Message.EqualTo(string.Format("Command {0} is incorrect", typeof(ICommand).Name))
        .And.Property("ValidationValidationStatus").SameAs(validationStatus));
    }

    [Test]
    public void DispatchAsync_InvoiceModelIsInvalid_CommandDispatcherDispatchMustNotHaveHappened()
    {
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus(new List<ValidationError> { new ValidationError("error") }));

      Assert.Throws<ValidationException>(() => _validatedCommandDispatcher.DispatchAsync(_command));

      A.CallTo(() => _commandDispatcher.DispatchAsync(A<ICommand>._)).MustNotHaveHappened();
    }

    [Test]
    public void DispatchAsync_InvoiceModelIsValid__CommandDispatcherDispatchMustHaveHappened()
    {
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus());

      _validatedCommandDispatcher.DispatchAsync(_command);

      A.CallTo(() => _commandDispatcher.DispatchAsync(_command)).MustHaveHappened();
    } 
  }
}