using PaymentGateway.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankServices.Interfaces
{
    public interface IAquiringBankPaymentService
    {
        public Task CompletePayment(SubmitPaymentCommand command);
    }
}
