﻿using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourierHubWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class InquireController : ControllerBase {
        private IInquireService _inquireService;
        public InquireController(IInquireService inquireService) {
            _inquireService = inquireService;
        }
        [HttpPost]
        public IActionResult CreateInquire(CreateInquireRequest request,
            [FromServices] IValidator<CreateInquireRequest> validator) {


            ModelStateDictionary? errors = this.Validate<CreateInquireRequest>(validator, request);
            if (errors != null)
                return ValidationProblem(errors);

            return this.ExtractServiceIdFromContext().Match(serviceId => _inquireService.CreateInquire(request, serviceId).Result.Match(
                                response => CreatedAtAction(
                                    actionName: nameof(CreateInquire),
                                    routeValues: new { id = response.Code },
                                    value: response),
                                errors => Problem()), errors => Problem());

        }

        [HttpPost("Email")]
        public IActionResult CreateInquireEmail(CreateInquireWithEmailRequest request,
            [FromServices] IValidator<CreateInquireWithEmailRequest> validator) {

            ModelStateDictionary? errors = this.Validate<CreateInquireWithEmailRequest>(validator, request);
            if (errors != null)
                return ValidationProblem(errors);

            return this.ExtractServiceIdFromContext().Match(serviceId => _inquireService.CreateInquireWithEmail(request, serviceId).Result.Match(
                                response => CreatedAtAction(
                                    actionName: nameof(CreateInquire),
                                    routeValues: new { id = response.Code },
                                    value: response),
                                errors => Problem()), errors => Problem());
        }
    }
}