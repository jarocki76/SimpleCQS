using System;
using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using NUnit.Framework;
using SimpleCQS.Command;
using SimpleCQS.Command.Validation;
using SimpleCQS.Exceptions;

namespace SimpleCQS.Tests.Command
{
  public class CommandExecutorTests
  {
    private ICommand _command;
    private ICommandHandler<ICommand> _handler;
    private IValidationProcessor _validationProcessor;

    [SetUp]
    public void SetUp()
    {
      _command = A.Fake<ICommand>();
      _handler = A.Fake<ICommandHandler<ICommand>>();
      _validationProcessor = A.Fake<IValidationProcessor>();
    }

    [Test]
    public void Execute_ValidateInvoiceCommand()
    {
      var commandDispatcher = new CommandExecutor(t => _handler, _validationProcessor);

      commandDispatcher.Execute(_command);

      A.CallTo(() => _validationProcessor.Validate(_command)).MustHaveHappened();
    }

    [Test]
    public void Execute_InvoiceModelIsInvalid_ValidationStateIsInvalid()
    {
      var commandDispatcher = new CommandExecutor(t => _handler, _validationProcessor);
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus(new List<ValidationError> { new ValidationError("error") }));

      var result = commandDispatcher.Execute(_command);

      Assert.IsFalse(result.IsValid);
    }

    [Test]
    public void Execute_InvoiceModelIsInvalid_HandlerHandleMustNotHaveHappened()
    {
      var commandDispatcher = new CommandExecutor(t => _handler, _validationProcessor);
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus(new List<ValidationError> { new ValidationError("error") }));

      commandDispatcher.Execute(_command);

      A.CallTo(() => _handler.Handle(A<ICommand>._)).MustNotHaveHappened();
    }

    [Test]
    public void Execute_InvoiceModelIsValid_ValidationStateIsValid()
    {
      var commandDispatcher = new CommandExecutor(t => _handler, _validationProcessor);
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus());

      var result = commandDispatcher.Execute(_command);

      Assert.IsTrue(result.IsValid);
    }

    [Test]
    public void Execute_CallsResolverWithExpectedType()
    {
      bool wasCalledWithExpectedType = false;
      var commandDispatcher = new CommandExecutor(t =>
      {
        if (t == typeof(ICommandHandler<ICommand>))
        {
          wasCalledWithExpectedType = true;
        }
        return _handler;
      }, _validationProcessor);
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus());

      commandDispatcher.Execute(_command);

      Assert.That(wasCalledWithExpectedType, Is.True);
    }

    [Test]
    public void Execute_QueryHandlerResolved_CallsHandleMethod()
    {
      var commandDispatcher = new CommandExecutor(t => _handler, _validationProcessor);
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus());

      commandDispatcher.Execute(_command);

      _handler.CallsTo(m => m.Handle(_command)).MustHaveHappened(Repeated.Exactly.Once);
    }

    [Test]
    public void Execute_NoHandlerDefined_ThrowsException()
    {
      var commandDispatcher = new CommandExecutor(t => { throw new Exception(); }, _validationProcessor);
      var expectedMessage = string.Format("Can not resolve handler for ICommandHandler<{0}>", typeof(ICommand).Name);
      A.CallTo(() => _validationProcessor.Validate(_command)).Returns(new ValidationStatus());

      Assert.That(() => commandDispatcher.Execute(_command), Throws.InstanceOf<ResolverException>().And.Message.EqualTo(expectedMessage));
    }
  }
}