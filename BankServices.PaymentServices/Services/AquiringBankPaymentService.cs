using BankServices.Domain.Commands;
using BankServices.Interfaces;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankServies.PaymentServices.Services
{
    public class AquiringBankPaymentService : IAquiringBankPaymentService
    {
        private readonly IBus _bus;

        public AquiringBankPaymentService(IBus bus)
        {
            _bus = bus;
        }

        public void CompletePayment(SubmitPaymentCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
