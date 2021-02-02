using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Controllers
{
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        public IActionResult Error()
        {
            var feature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (feature?.Error is NotFoundException notFoundException)
            {
                _logger.LogDebug(feature.Error, feature.Error.Message);
                return NotFound(new 
                {
                    Success = false,
                    Message = feature.Error.Message,
                });
            }
            else if (feature?.Error is ValidationException validationException)
            {
                _logger.LogDebug(feature.Error, feature.Error.Message);
                return BadRequest(new
                {
                    Success = false,
                    Message = "The request was invalid",
                    InvalidFields = validationException.Data,
                });
            }
            else
            {
                _logger.LogError(feature.Error, feature.Error.Message);
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                return new ObjectResult(new
                {
                    Success = false,
                    Message = "There was an error, if the isuse persists, please contact support",
                });
            }
        }
    }
}
