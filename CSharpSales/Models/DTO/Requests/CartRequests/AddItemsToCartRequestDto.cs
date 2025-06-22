using CSharpSales.Models.DTO.Responses.CartResponses;
using MediatR;

namespace CSharpSales.Models.DTO.Requests.CartRequests
{
    public class AddItemsToCartRequestDto : IRequest<GetCartResponseDto>
    {
        public List<string> Items { get; set; } = [];
    }
}
