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
        public async Task<IActionResult> SubmitPayment(PaymentRequestModel model)
        {
            var id = await _paymentService.SubmitPayment(model);

            return RedirectToAction(nameof(GetPaymentRecordById), new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentRecordById(int id)
        {
            return Ok(await _paymentService.GetPaymentRecord(id));
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPaymentRecordByBankPaymentId(Guid? paymentId)
        {
            if (!paymentId.HasValue)
                return BadRequest(new { message = "payment ID not provided" });
            return Ok(await _paymentService.GetPaymentRecord(paymentId.Value));
        }
    }
}
