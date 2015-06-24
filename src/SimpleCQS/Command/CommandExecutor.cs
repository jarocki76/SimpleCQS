using System;
using SimpleCQS.Command.Validation;
using SimpleCQS.Exceptions;

namespace SimpleCQS.Command
{
  public class CommandExecutor : ICommandExecutor
  {
    private readonly Func<Type, object> _resolver;
    private readonly IValidationProcessor _validationProcessor;

    public CommandExecutor(Func<Type, object> resolver, IValidationProcessor validationProcessor)
    {
      _resolver = resolver;
      _validationProcessor = validationProcessor;
    }

    public virtual IValidationStatus Execute<T>(T command) where T : ICommand
    {
      var validationStatus = _validationProcessor.Validate(command);
      if (!validationStatus.IsValid)
      {
        return validationStatus;
      }

      ICommandHandler<T> handler;
      try
      {
        handler = (ICommandHandler<T>)_resolver(typeof(ICommandHandler<T>));
      }
      catch (Exception ex)
      {
        throw new ResolverException(string.Format("Can not resolve handler for ICommandHandler<{0}>", typeof(T).Name), ex);
      }
      
      handler.Handle(command);

      return new ValidationStatus();
    }
  }
}