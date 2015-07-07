using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using NUnit.Framework;
using SimpleCQS.Exceptions;
using SimpleCQS.Query;

namespace SimpleCQS.Tests.Query
{
  public class QueryExecutorTests
  {
    private IQuery<object> _query;
    private IQueryHandler<IQuery<object>, object> _handler;

    [SetUp]
    public void SetUp()
    {
      _query = A.Fake<IQuery<object>>();
      _handler = A.Fake<IQueryHandler<IQuery<object>, object>>();
    }

    [Test]
    public void Execute_CallsResolverWithExpectedType()
    {
      bool wasCalledWithExpectedType = false;
      var queryExecutor = new QueryExecutor(t =>
      {
        if (t == typeof(IQueryHandler<IQuery<object>, object>))
        {
          wasCalledWithExpectedType = true;
        }
        return _handler;
      });

      queryExecutor.Execute<IQuery<object>, object>(_query);

      Assert.That(wasCalledWithExpectedType, Is.True);
    }

    [Test]
    public void Execute_QueryHandlerResolved_CallsHandleMethod()
    {
      var queryExecutor = new QueryExecutor(t => _handler);
      
      queryExecutor.Execute<IQuery<object>, object>(_query);

      _handler.CallsTo(m => m.Handle(_query)).MustHaveHappened(Repeated.Exactly.Once);
    }

    [Test]
    public void Execute_NoHandlerDefined_ThrowsException()
    {
      var queryExecutor = new QueryExecutor(t => {throw new Exception();});
      var expectedMessage = string.Format("Can not resolve handler for IQueryHandler<{0}, {1}>", typeof(IQuery<object>).Name, typeof(object).Name);

      Assert.That(() => queryExecutor.Execute<IQuery<object>, object>(_query), Throws.InstanceOf<ResolverException>().And.Message.EqualTo(expectedMessage));
    }
  }
}