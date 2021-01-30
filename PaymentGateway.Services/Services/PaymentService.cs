using AutoMapper;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Models;
using PaymentGateway.Services.Exceptions;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Services.Models;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public PaymentService(IPaymentsRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IBus bus)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bus = bus;
        }

        public async Task<PaymentRecordModel> GetPaymentRecord(int id)
        {
            var payment = await _repository.GetPayment(id);
            if (payment == null) throw new NotFoundException(nameof(Payment), id);

            var paymentRecord = _mapper.Map<PaymentRecordModel>(payment);
            return paymentRecord;
        }

        public async Task<PaymentRecordModel> GetPaymentRecord(Guid bankPaymentId)
        {
            var payment = await _repository.GetPayment(bankPaymentId);
            if (payment == null) throw new NotFoundException(nameof(Payment), bankPaymentId);

            var paymentRecord = _mapper.Map<PaymentRecordModel>(payment);
            return paymentRecord;
        }

        public async Task<int> SubmitPayment(PaymentRequestModel requestModel)
        {

            var payment = _mapper.Map<Payment>(requestModel);
            await _repository.AddPayment(payment);
            await _unitOfWork.Commit();

            var command = _mapper.Map<SubmitPaymentCommand>(requestModel);
            await _bus.Publish(command);

            return payment.Id;
        }
    }
}
