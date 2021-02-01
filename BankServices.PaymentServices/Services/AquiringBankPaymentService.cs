using BankServices.Interfaces;
using PaymentGateway.Messages.Commands;
using PaymentGateway.Messages.Events;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankServies.PaymentServices.Services
{
    public class AquiringBankPaymentService : IAquiringBankPaymentService
    {
        private readonly IBus _bus;

        public AquiringBankPaymentService(IBus bus)
        {
            _bus = bus;
        }

        public async Task CompletePayment(SubmitPaymentCommand command)
        {
            await Task.Delay(1000);

            await _bus.Publish(new PaymentCompletedEvent
            {
                PaymentId = command.Id,
                BankPaymentId = Guid.NewGuid(),
                Success = new Random().Next(0, 2) == 0
            });
        }
    }
}
