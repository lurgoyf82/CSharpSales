using CSharpSalesBE.Models.Entities;
using System;

namespace CSharpSalesBE.Services
{
    public class TaxCalculator
    {
        public static decimal CalculateTaxes(Item item)
        {
            decimal taxes = (item.Price / 100m) * item.TaxPercentage;
            if (taxes > 0)
            {
                // Round up to the nearest 0.05 
                taxes = Math.Ceiling(taxes * 20) / 20;
            }

            return taxes;
        }
    }
}