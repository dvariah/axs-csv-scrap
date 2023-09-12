using System.IO.Compression;

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

        public static void WriteExtractedSalesFile(string filePath, List<Sale> extractedSales)
        {
            ConsoleTracer.TraceMethodEntry($"Enter WriteExtractedSalesFile method. FilePath {filePath}, extractedSales count: {extractedSales.Count}");

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            DeleteFileIfExists(filePath);

            using var file = File.Create(filePath);
            using var compress = new GZipStream(file, CompressionMode.Compress);
            using var writer = new StreamWriter(compress);
            foreach (var sale in extractedSales)
            {
                writer.WriteLine(sale.OriginalCsvLine);
            }

            ConsoleTracer.TraceMethodEntry($"Exit WriteExtractedSalesFile method.");
        }

        public static void WriteExtractedPamentsFile(string filePath, List<Payment> extractedPayments)
        {
            ConsoleTracer.TraceMethodEntry($"Enter WriteExtractedPamentsFile method. FilePath {filePath}, extractedPayments count: {extractedPayments.Count}");

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            DeleteFileIfExists(filePath);

            using var file = File.Create(filePath);
            using var compress = new GZipStream(file, CompressionMode.Compress);
            using var writer = new StreamWriter(compress);
            foreach (var payment in extractedPayments)
            {
                writer.WriteLine(payment.OriginalCsvLine);
            }

            ConsoleTracer.TraceMethodEntry($"Exit WriteExtractedPamentsFile method.");
        }

        public static void WriteExtractedDistributionFile(string filePath, List<Distribution> extractedDistributions)
        {
            ConsoleTracer.TraceMethodEntry($"Enter WriteExtractedDistributionFile method. FilePath {filePath}, extractedDistributions count: {extractedDistributions.Count}");

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            DeleteFileIfExists(filePath);

            using var file = File.Create(filePath);
            using var compress = new GZipStream(file, CompressionMode.Compress);
            using var writer = new StreamWriter(compress);
            foreach (var distribution in extractedDistributions)
            {
                writer.WriteLine(distribution.OriginalCsvLine);
            }

            ConsoleTracer.TraceMethodEntry($"Exit WriteExtractedDistributionFile method.");
        }

        public static void WriteControlCard(string filePath, string message)
        {
            DeleteFileIfExists(filePath);
            using var writer = File.CreateText(filePath);
            writer.WriteLine(message);
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
