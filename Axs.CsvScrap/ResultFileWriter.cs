namespace Axs.CsvScrap
{
    public class ResultFileWriter
    {
        public static void WriteStatsFile(string filePath, List<SaleStats> saleStats)
        {
            ConsoleTracer.TraceMethodEntry($"Enter WriteStatsFile method. FilePath {filePath}, saleStats count: {saleStats.Count}");
            DeleteFileIfExists(filePath);
            using var writer = File.CreateText(filePath);

            var header = "order_id, num_of_ticket_sales, num_of_fee_sales, num_of_merch_sales, num_of_payments, num_of_paymentdistributions";
            writer.WriteLine(header);

            foreach (var result in saleStats)
            {
                var resultString = $"{result.order_id}, {result.num_of_ticket_sales}, {result.num_of_fee_sales}, {result.num_of_merch_sales}, {result.num_of_payments}, {result.num_of_paymentdistributions}";
                writer.WriteLine(resultString);
            }
            ConsoleTracer.TraceMethodEntry($"Exit WriteStatsFile method.");
        }

        private static void DeleteFileIfExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
