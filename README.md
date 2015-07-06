# SimpleCQS [![Build status](https://ci.appveyor.com/api/projects/status/aneojew2ehsgaijo/branch/master?retina=true)](https://ci.appveyor.com/project/JarosawLeszczyski/simplecqs/branch/master)

Library to simple command query separation



# NuGet
```
Install-Package SimpleCQS
```

# Simple Example with Autofac

```
Install-Package SimpleCQS.Autofac
```
###### Commands and Handlers and Validators
```
public class MyCommand : ICommand
{
  public string CommandProp1 { get; set; }
  public int CommandProp2 { get; set; }
  //(...)
}
  
public class MyCommandHandler : ICommandHandler<MyCommand>
{
  public MyCommandHandler( /* dependencies */)
  {
  }

  public void Handle(MyCommand command)
  {
    //do something with command
  }
}

public class MyCommandValidator : AbstractValidator<MyCommand>
{
  public MyCommandValidator()
  {
    RuleFor(x => x.CommandProp1)
      .NotEmpty();

    RuleFor(x => x.CommandProp2)
      .GreaterThan(0);

    //(...)
  }
}
```
###### Autofac configuration
```
var builder = new ContainerBuilder();
builder.RegisterSimpleCQS(AppDomain.CurrentDomain.GetAssemblies()); //Assemblies with handlers and validators
var container = builder.Build();
```
###### Program
```
var commandBus = container.Resolve<ICommandExecutor>(); //Executor with validation result
//var commandBus = container.Resolve<ICommandDispatcher>(); //Dispather with validation
//var commandBus = container.ResolveNamed<ICommandDispatcher>("SimpleDispatcher") //Dispather without validation;

var command = new MyCommand()
{
  CommandProp1 = "Prop1",
  CommandProp2 = 1
};

var validationResult = commandBus.Execute(command) //for Executor with validation result;
//commandBus.Dispatch(command); //for Dispatcher with and without validation
```
