using System.Threading.Tasks;
using SimpleCQS.Command.Validation;

namespace SimpleCQS.Command
{
  public class ValidatedCommandDispatcher : ICommandDispatcher
  {
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IValidationProcessor _validationProcessor;

    public ValidatedCommandDispatcher(ICommandDispatcher commandDispatcher, IValidationProcessor validationProcessor)
    {
      _commandDispatcher = commandDispatcher;
      _validationProcessor = validationProcessor;
    }

    public void Dispatch<T>(T command) where T : ICommand
    {
      Validate(command);

      _commandDispatcher.Dispatch(command);
    }

    public Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
      Validate(command);

      return _commandDispatcher.DispatchAsync(command);
    }

    private void Validate<T>(T command) where T : ICommand
    {
      var valiationStatus = _validationProcessor.Validate(command);
      if (!valiationStatus.IsValid)
      {
        var message = string.Format("Command {0} is incorrect", typeof(T).Name);
        throw new ValidationException(message, valiationStatus);
      }
    }
  }
}