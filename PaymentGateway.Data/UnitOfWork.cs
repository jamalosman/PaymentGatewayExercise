using PaymentGateway.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PaymentsContext _context;

        public UnitOfWork(PaymentsContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
