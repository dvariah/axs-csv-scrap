namespace Axs.CsvScrap
{
    public class Distribution
    {
        public string unique_id { get; set; }
        public string payment_id { get; set; }
        public decimal distribution_amount { get; set; }

        public string OriginalCsvLine { get; set; }
    }
}
