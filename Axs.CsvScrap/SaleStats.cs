namespace Axs.CsvScrap
{
    public class SaleStats
    {
        public string order_id { get; set; }
        public int num_of_ticket_sales { get; set; }
        public int num_of_fee_sales { get; set; }
        public int num_of_merch_sales { get; set; }
        public int num_of_payments { get; set; }
        public int num_of_paymentdistributions { get; set; }
        public string venue { get; set; }
        public string client { get; set; }
        public string event_name { get; set; }
        public string zone_type { get; set; }
        public string price_code_type { get; set; }
        public string merch_id { get; set; }
        public string outlet { get; set; }
        public string payment_type { get; set; }
    }
}
