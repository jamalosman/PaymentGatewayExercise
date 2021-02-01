using System;
using System.Runtime.Serialization;

namespace PaymentGateway.Services.Exceptions
{
    [Serializable]
    internal class UnauthorizedResourceAccessException : Exception
    {
        public UnauthorizedResourceAccessException()
        {
        }

        public UnauthorizedResourceAccessException(string message) : base(message)
        {
        }

        public UnauthorizedResourceAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthorizedResourceAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}