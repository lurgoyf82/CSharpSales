using CSharpSalesBE.Models.DTO.Requests.CartRequests;
using CSharpSalesBE.Models.DTO.Responses.CartResponses;
using CSharpSalesBE.Models.Entities;
using CSharpSalesBE.Services;
using MediatR;

namespace CSharpSalesBE.Handlers
{
    public class GetCartResponseHandler : IRequestHandler<AddItemsToCartRequestDto, GetCartResponseDto>
    {
        private readonly ILogger<GetCartResponseHandler> _logger;
        private readonly IMediator _mediator;
        private readonly CartOutputFormatter _outputFormatter;
        private readonly CartService _cartService;
        private readonly InputParser _inputParser;

        public GetCartResponseHandler(ILogger<GetCartResponseHandler> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _outputFormatter = new CartOutputFormatter();
            _cartService = new CartService();
            _inputParser = new InputParser();
        }

        public async Task<GetCartResponseDto> Handle(AddItemsToCartRequestDto request, CancellationToken cancellationToken)
        {


            if (request == null || request.Items == null || !request.Items.Any())
            {
                throw new ArgumentException("Request cannot be null or empty.");
            }

            var cart = new Cart();

            foreach (var itemText in request.Items)
            {
                _inputParser.SetText(itemText);
                var item = _inputParser.Parse();
                _cartService.AddItem(cart, item);
            }

            return _outputFormatter.GetCartResponse(cart);
        }
    }
}
