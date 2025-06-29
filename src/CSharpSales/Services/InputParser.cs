using CSharpSales.Models.Entities;
using CSharpSales.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace CSharpSales.Services
{
    public class InputParser
    {
        private readonly Dictionary<string, string> ItemType = new()
        {
            { "book", "book" },
            { "music CD", "other" },
            { "chocolate bar", "food" },
            { "box of chocolates", "food" },
            { "bottle of perfume", "other" },
            { "packet of headache pills", "medical_products" }
        };

        private string? _text;

        public InputParser(string? text = null)
        {
            _text = text;
        }

        public void SetText(string text)
        {
            _text = text;
        }

        public Item Parse()
        {
            if (string.IsNullOrWhiteSpace(_text))
                throw new ArgumentException("Il testo inserito è invalido.");

            //example: "3 box of imported chocolates at 11.25"
            var item = new Item();

            // Quantità
            var quantityEndCharPos = _text.IndexOf(' ');
            item.Quantity = int.Parse(_text.Substring(0, quantityEndCharPos));


            // Prezzo
            var atFirstCharPos = _text.LastIndexOf("at", StringComparison.OrdinalIgnoreCase);
            item.Price = decimal.Parse(_text.Substring(atFirstCharPos + 2).Trim(), CultureInfo.InvariantCulture);

            var nameText = _text.Substring(quantityEndCharPos + 1, (atFirstCharPos - quantityEndCharPos) - 1).Trim();

            item.Imported = nameText.Contains("imported", StringComparison.OrdinalIgnoreCase);

            if (item.Imported)
            {
                var importedIndex = nameText.IndexOf("imported", StringComparison.OrdinalIgnoreCase);
                if (importedIndex >= 0)
                {
                    var before = nameText.Substring(0, importedIndex).Trim();
                    var after = nameText.Substring(importedIndex + "imported".Length).Trim();
                    nameText = (before + " " + after).Trim();
                }

                // Nome
                item.Name = "imported " + nameText;
                item.TaxPercentage = Taxes.Map["import"];
            }
            else
            {
                // Nome
                item.Name = nameText;
                item.TaxPercentage = 0;
            }

            // Tipo di item
            item.ItemType = ItemType.ContainsKey(nameText) ? ItemType[nameText] : "other";

            // Calcolo le tasse
            item.TaxPercentage += Taxes.Map[item.ItemType];

            item.Taxes = item.Quantity * TaxCalculator.CalculateTaxes(item);
            item.TaxedPrice = (item.Quantity * item.Price) + item.Taxes;

            return item;
        }
    }
}