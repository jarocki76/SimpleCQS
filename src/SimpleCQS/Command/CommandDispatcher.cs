using System;
using System.Threading.Tasks;
using SimpleCQS.Exceptions;

namespace SimpleCQS.Command
{
  public class CommandDispatcher : ICommandDispatcher
  {
    private readonly Func<Type, object> _resolver;

    public CommandDispatcher(Func<Type, object> resolver)
    {
      _resolver = resolver;
    }

    public virtual void Dispatch<T>(T command) where T : ICommand
    {
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
    }

    public Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
      var task = new Task(() => Dispatch(command));
      task.Start();
      return task;
    }
  }
}