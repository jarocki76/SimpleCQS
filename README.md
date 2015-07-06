# SimpleCQS
Library to simple command query separation

<img ng-src="https://ci.appveyor.com/api/projects/status/aneojew2ehsgaijo?svg=true" src="https://ci.appveyor.com/api/projects/status/aneojew2ehsgaijo?svg=true">

# NuGet
```
Install-Package SimpleCQS
```

# Simple Example with Autofac

```
Install-Package SimpleCQS.Autofac
```
###### Commands and Handlers
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
builder.RegisterSimpleCQS(AppDomain.CurrentDomain.GetAssemblies());
var container = builder.Build();
```
###### Program
```
var commandBus = container.Resolve<ICommandDispatcher>();

var command = new MyCommand()
{
  CommandProp1 = "Prop1",
  CommandProp2 = 1
};

commandBus.Dispatch(command);
```
