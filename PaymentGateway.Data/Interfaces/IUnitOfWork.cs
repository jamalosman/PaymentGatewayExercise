using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Interfaces
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
