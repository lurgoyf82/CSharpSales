using CSharpSalesBE.Models.DTO.Responses.CartResponses;
using MediatR;

namespace CSharpSalesBE.Models.DTO.Requests.CartRequests
{
    public class AddItemsToCartRequestDto : IRequest<GetCartResponseDto>
    {
        public List<string> Items { get; set; } = [];
    }
}
