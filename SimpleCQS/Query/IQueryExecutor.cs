namespace SimpleCQS.Query
{
  public interface IQueryExecutor
  {
    TR Execute<TQ, TR>(TQ query) where TQ : IQuery<TR> where TR : class;
  }
}