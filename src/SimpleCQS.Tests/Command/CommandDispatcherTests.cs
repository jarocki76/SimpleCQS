using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using NUnit.Framework;
using SimpleCQS.Command;
using SimpleCQS.Exceptions;

namespace SimpleCQS.Tests.Command
{
  public class CommandDispatcherTests
  {
    private ICommand _command;
    private ICommandHandler<ICommand> _handler;

    [SetUp]
    public void SetUp()
    {
      _command = A.Fake<ICommand>();
      _handler = A.Fake<ICommandHandler<ICommand>>();
    }

    [Test]
    public void Dispatch_CallsResolverWithExpectedType()
    {
      bool wasCalledWithExpectedType = false;
      var commandDispatcher = new CommandDispatcher(t =>
      {
        if (t == typeof(ICommandHandler<ICommand>))
        {
          wasCalledWithExpectedType = true;
        }
        return _handler;
      });

      commandDispatcher.Dispatch(_command);

      Assert.That(wasCalledWithExpectedType, Is.True);
    }

    [Test]
    public void Dispatch_QueryHandlerResolved_CallsHandleMethod()
    {
      var commandDispatcher = new CommandDispatcher(t => _handler);

      commandDispatcher.Dispatch(_command);

      _handler.CallsTo(m => m.Handle(_command)).MustHaveHappened(Repeated.Exactly.Once);
    }

    [Test]
    public void Dispatch_NoHandlerDefined_ThrowsException()
    {
      var commandDispatcher = new CommandDispatcher(t => { throw new Exception(); });
      var expectedMessage = string.Format("Can not resolve handler for ICommandHandler<{0}>", typeof(ICommand).Name);

      Assert.That(() => commandDispatcher.Dispatch(_command), Throws.InstanceOf<ResolverException>().And.Message.EqualTo(expectedMessage));
    }

    [Test]
    public void ExecuteAsync_CallsExecuteInTask()
    {
      ICommand command = A.Fake<ICommand>();
      var commandDispatcher = A.Fake<CommandDispatcher>();
      commandDispatcher.CallsTo(m => m.Dispatch(command));

      var task = commandDispatcher.DispatchAsync(command);
      task.Wait();

      commandDispatcher.CallsTo(m => m.Dispatch(command)).MustHaveHappened(Repeated.Exactly.Once);
    }

    [Test]
    public void ExecuteAsync_NoHandlerDefined_ThrowsException()
    {
      var commandDispatcher = new CommandDispatcher(t => { throw new Exception(); });
      var expectedMessage = string.Format("Can not resolve handler for ICommandHandler<{0}>", typeof(ICommand).Name);

      Assert.That(async () => await commandDispatcher.DispatchAsync(_command), Throws.InstanceOf<ResolverException>().And.Message.EqualTo(expectedMessage));
    }
  }
}