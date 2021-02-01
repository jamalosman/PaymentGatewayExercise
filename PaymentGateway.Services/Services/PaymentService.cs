using AutoMapper;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Messages.Commands;
using PaymentGateway.Services.Exceptions;
using PaymentGateway.Services.Extensions;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Services.Models;
using PaymentGateway.Services.Validators;
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
        private readonly PaymentRequestValidator _validator;

        public PaymentService(
            IPaymentsRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBus bus,
            PaymentRequestValidator validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bus = bus;
            _validator = validator;
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
            // TODO: remove this line once auth is implemented
            requestModel.MerchantId = 1;

            _validator.ValidateForServices(requestModel);
            
            var payment = _mapper.Map<Payment>(requestModel);
            payment.Submitted = DateTime.Now;
            payment.Status = PaymentStatus.NotSubmitted;
            await _repository.AddPayment(payment);
            await _unitOfWork.Commit();

            var command = _mapper.Map<SubmitPaymentCommand>(payment);
            await _bus.Publish(command);

            payment.Status = PaymentStatus.Submitted;
            await _unitOfWork.Commit();

            return payment.Id;
        }

        public async Task UpdatePayment(int id, Guid bankPaymentId, bool success)
        {
            var payment = await _repository.GetPayment(id);
            if (payment == null) throw new NotFoundException(nameof(Payment), id);

            payment.BankPaymentId = bankPaymentId;
            payment.Status = success ? PaymentStatus.Success : PaymentStatus.Failure;
            payment.Processed = DateTime.Now;

            await _repository.UpdatePayment(payment);
            await _unitOfWork.Commit();
        }
    }
}
