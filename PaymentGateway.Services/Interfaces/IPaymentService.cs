﻿using PaymentGateway.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// record a payment submission and submit to aquiring bank for processing
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public Task<int> SubmitPayment(PaymentRequestModel requestModel);

        /// <summary>
        /// Retrieve a payment record via ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PaymentRecordModel> GetPaymentRecord(int id);

        /// <summary>
        /// retrieve a payment via the ID generated by the bank
        /// </summary>
        /// <param name="bankPaymentId"></param>
        /// <returns></returns>
        public Task<PaymentRecordModel> GetPaymentRecord(Guid bankPaymentId);

        /// <summary>
        /// Update a payment after it is processed by the aquiring bank
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bankPaymentId"></param>
        /// <returns></returns>
        Task UpdatePayment(int id, Guid bankPaymentId, bool success);
    }
}
