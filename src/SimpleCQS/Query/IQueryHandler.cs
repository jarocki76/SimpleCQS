namespace SimpleCQS.Query
{
  public interface IQueryHandler<in TQ, out TR> where TQ : IQuery<TR> where TR : class
  {
    TR Handle(TQ query);
  }
}