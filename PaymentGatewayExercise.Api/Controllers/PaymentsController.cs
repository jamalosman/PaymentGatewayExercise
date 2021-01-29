using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public IActionResult SubmitPayment(PaymentRequestModel model)
        {
            var id = _paymentService.SubmitPayment(model);

            return RedirectToAction(nameof(GetPaymentRecordById), new { id });
        }

        [HttpGet("{id}")]
        public IActionResult GetPaymentRecordById(int id)
        {
            return Ok(_paymentService.GetPaymentRecord(id));
        }

        [HttpGet("")]
        public IActionResult GetPaymentRecordByBankPaymentId(Guid paymentId)
        {
            return Ok(_paymentService.GetPaymentRecord(paymentId));
        }
    }
}
