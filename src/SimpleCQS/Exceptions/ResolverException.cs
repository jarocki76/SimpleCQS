using System;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCQS.Exceptions
{
  [ExcludeFromCodeCoverage]
  public class ResolverException : Exception
  {
    public ResolverException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}