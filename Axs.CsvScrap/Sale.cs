namespace Axs.CsvScrap
{
    public class Sale
    {
        public string unique_id { get; set; }
        public string transaction_id { get; set; }
        public string inventory_type { get; set; }
        public decimal total_sales_gross_amount { get; set; }

        public string venue { get; set; }
        public string client { get; set; }
        public string event_name { get; set; }
        public string zone_type { get; set; }
        public string price_code_type { get; set; }

        public string OriginalCsvLine { get; set; }
    }
}
