using System;
using System.Diagnostics.CodeAnalysis;

namespace SimpleCQS.Command.Validation
{
  [ExcludeFromCodeCoverage]
  public class ProperlyValidatorIsNotRegisteredException : Exception
  {
    public ProperlyValidatorIsNotRegisteredException(string message)
      : base(message)
    {
    }
  }
}