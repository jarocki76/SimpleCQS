# SimpleCQS
Library to simple command query separation

<img ng-src="https://ci.appveyor.com/api/projects/status/aneojew2ehsgaijo?svg=true" src="https://ci.appveyor.com/api/projects/status/aneojew2ehsgaijo?svg=true">

# NuGet
```
Install-Package SimpleCQS
```

# Simple Example with Autofac

```
Install-Package SimpleCQS
Install-Package Autofac
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
  public void Handle(MyCommand command)
  {
    //DoSomething
  }
}
```
###### Autofac configuration
```
var builder = new ContainerBuilder();
builder.Register(ctx =>
{
  var c = ctx.Resolve<IComponentContext>();
  Func<Type, object> resolver = c.Resolve;
  return resolver;
}).As<Func<Type, object>>().SingleInstance();
builder.RegisterType<CommandDispatcher>()
  .As<ICommandDispatcher>()
  .SingleInstance();
builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
  .AsClosedTypesOf(typeof(ICommandHandler<>));
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
