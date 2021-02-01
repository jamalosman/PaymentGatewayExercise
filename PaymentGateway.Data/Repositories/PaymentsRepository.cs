using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly PaymentsContext _context;

        public PaymentsRepository(PaymentsContext context)
        {
            _context = context;
        }

        public async Task AddPayment(Payment payment)
        {
            await _context.Set<Payment>().AddAsync(payment);
        }

        public async Task UpdatePayment(Payment payment)
        {
            var existingPayment = await _context.Set<Payment>().FindAsync(payment.Id);
            _context.Entry<Payment>(existingPayment).CurrentValues.SetValues(payment);
        }

        public async Task<Payment> GetPayment(int id)
        {
            return await _context.Set<Payment>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Payment> GetPayment(Guid bankPaymentId)
        {
            return await _context.Set<Payment>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BankPaymentId == bankPaymentId);
        }
    }
}
