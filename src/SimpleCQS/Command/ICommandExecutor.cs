using SimpleCQS.Command.Validation;

namespace SimpleCQS.Command
{
  public interface ICommandExecutor
  {
    IValidationStatus Execute<T>(T command) where T : ICommand;
  }
}