namespace Axs.CsvScrap
{
    public class Sale
    {
        public string unique_id { get; set; }
        public string transaction_id { get; set; }
        public string inventory_type { get; set; }
        public decimal total_sales_gross_amount { get; set; }

        public string OriginalCsvLine { get; set; }
    }
}
