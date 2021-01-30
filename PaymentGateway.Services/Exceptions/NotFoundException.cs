using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services.Exceptions
{
    public class NotFoundException : Exception
    {
        public string EntityName { get; }
        public object Id { get; }
        public NotFoundException() : base($"Could not find entity")
        {

        }

        public NotFoundException(string entityName, object id) : base($"Could not find {entityName} with id {id}")
        {
            EntityName = entityName;
            Id = id;
        }
    }
}
