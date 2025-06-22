using CSharpSales.Models.DTO.Requests.CartRequests;
using CSharpSales.Models.DTO.Responses.CartResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace CSharpSales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : Controller
    {

        private readonly IMediator _mediator;
        private readonly ILogger<SalesController> _logger;


        public SalesController(IMediator mediator, ILogger<SalesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }



        [HttpPost]
        [Route("/[action]")]
        [ProducesResponseType(typeof(GetCartResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerRequestExample(typeof(AddItemsToCartRequestDto), typeof(AddItemsToCartRequestDtoExample))]
        public async Task<IActionResult> GetCartResponseAsync([FromBody] AddItemsToCartRequestDto request)
        {
            GetCartResponseDto response = new();
            try
            {
                response = await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
            return Ok(response);
        }
    }
}
