using System;

namespace Product.Application.Exceptions
{
    public class UnhandledException : ApplicationException
    {
        public UnhandledException(string name, object key)
            : base($"An Unknown error occurred for \"{ name }\" ({key})")
        {
        }
    }
}