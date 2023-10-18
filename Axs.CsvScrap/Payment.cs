namespace Axs.CsvScrap
{
    public class Payment
    {
        public string unique_id { get; set; }
        public string transaction_id { get; set; }
        public string payment_id { get; set; }
        public decimal payment_amount { get; set; }

        public string merch_id { get; set; }
        public string outlet { get; set; }
        public string payment_type { get; set; }

        public string OriginalCsvLine { get; set; }
    }
}
