namespace CSharpSales.Models.Entities
{
    public static class Taxes
    {
        public const decimal Book = 0m;
        public const decimal Food = 0m;
        public const decimal MedicalProducts = 0m;
        public const decimal Other = 10m;
        public const decimal ImportDuty = 5m;


        public static readonly Dictionary<string, decimal> Map = new()
        {
            { "book", Book },
            { "food", Food },
            { "medical_products", MedicalProducts },
            { "other", Other },
            { "import", ImportDuty }
        };
    }
}
