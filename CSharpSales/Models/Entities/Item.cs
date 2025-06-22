namespace CSharpSales.Models.Entities
{
    public class Item
    {
        public string ItemType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public decimal TaxedPrice { get; set; }
        public decimal TaxPercentage { get; set; }
        public int Quantity { get; set; }
        public bool Imported { get; set; }
    }
}
