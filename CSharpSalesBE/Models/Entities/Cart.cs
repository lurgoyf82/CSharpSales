namespace CSharpSalesBE.Models.Entities
{
    public class Cart
    {
        public List<Item> Items { get; set; } = [];
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
    }
}
