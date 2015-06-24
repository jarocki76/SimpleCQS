using System.Threading.Tasks;

namespace SimpleCQS.Command
{
  public interface ICommandDispatcher
  {
    void Dispatch<T>(T command) where T : ICommand;

    Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
  }
}