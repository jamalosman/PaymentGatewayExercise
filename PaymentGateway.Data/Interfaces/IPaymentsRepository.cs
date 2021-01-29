using PaymentGateway.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Interfaces
{
    public interface IPaymentsRepository
    {
        Task AddPayment(Payment payment);
        Task UpdatePayment(Payment payment);
        Task<Payment> GetPayment(int id);
        Task<Payment> GetPayment(Guid bankPaymentId);
    }
}
