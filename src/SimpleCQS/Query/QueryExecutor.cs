using System;
using SimpleCQS.Exceptions;

namespace SimpleCQS.Query
{
  public class QueryExecutor : IQueryExecutor
  {
    private readonly Func<Type, object> _resolver;

    public QueryExecutor(Func<Type, object> resolver)
    {
      _resolver = resolver;
    }

    public TR Execute<TQ, TR>(TQ query) where TQ : IQuery<TR> where TR : class
    {
      IQueryHandler<TQ, TR> handler;
      try
      {
        var o = _resolver(typeof(IQueryHandler<TQ, TR>));
        handler = (IQueryHandler<TQ, TR>)o;
      }
      catch (Exception ex)
      {
        throw new ResolverException(string.Format("Can not resolve handler for IQueryHandler<{0}, {1}>", typeof(TQ).Name, typeof(TR).Name), ex);
      }

      var result = handler.Handle(query);

      return result;
    }
  }
}