namespace TRMApi.Models
{
    public class CreateProductModel
    {
        public string ProductName { get; set; }

        public string Description { get; set; }

        public decimal RetailPrice { get; set; }

        public int QuantityInStock { get; set; }

        public bool IsTaxable { get; set; }

        public string ProductImage { get; set; }

    }
}
