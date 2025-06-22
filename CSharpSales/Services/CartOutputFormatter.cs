using CSharpSales.Models.DTO.Responses.CartResponses;
using CSharpSales.Models.Entities;
using System;

namespace CSharpSales.Services
{
    public class CartOutputFormatter
    {
        private static string FormatItem(Item item)
        {
            return $"{item.Quantity} {item.Name}: {item.TaxedPrice:F2}";
        }

        public List<string> PrintCart(Cart cart)
        {
            //var receiptLines = cart.Items.Select(FormatItem).ToList();

            var receiptLines = new List<string>();
            foreach (var item in cart.Items)
            {
                receiptLines.Add(FormatItem(item));
            }

            receiptLines.Add($"Sales Taxes: {cart.Tax:F2}");
            receiptLines.Add($"Total: {cart.Total:F2}");

            return receiptLines;
        }

        public GetCartResponseDto GetCartResponse(Cart cart)
        {
            var response = new GetCartResponseDto();
            foreach (var item in cart.Items)
            {
                response.Items.Add(FormatItem(item));
            }

            //response.SalesTaxes = decimal.Parse(cart.Tax.ToString("F2"));
            //response.Total = decimal.Parse(cart.Total.ToString("F2"));

            response.SalesTaxes=cart.Tax;
            response.Total=cart.Total;

            return response;
        }
    }
}