using Axs.CsvScrap;

class Program
{
    static void Main(String[] Args)
    {
        var reader = new Reader();
        var helper = new ArgHelper();

        helper.Parse(Args);

        if (helper.isCallForHelp)
        {
            Console.WriteLine(ArgHelper.DOC);
        }

        var watch = System.Diagnostics.Stopwatch.StartNew();

        if (helper.getFileStats)
        {
            var results = new List<SaleStats>();
            var sales = reader.ReadSales(helper.SalesFilePath);
            var payments = reader.ReadPayments(helper.PaymentFilePath);
            var distributions = reader.ReadDistributions(helper.DistributionFilePath);

            foreach (var sale in sales)
            {
                var stats = results.FirstOrDefault(r => r.order_id.Equals(sale.transaction_id));
                if (stats == null)
                {
                    stats = new SaleStats { order_id = sale.transaction_id };
                    results.Add(stats);
                }

                switch (sale.inventory_type)
                {
                    case "0":
                        stats.num_of_fee_sales++;
                        break;
                    case "1":
                        stats.num_of_merch_sales++;
                        break;
                    case "2":
                        stats.num_of_ticket_sales++;
                        break;
                }
            }

            foreach (var payment in payments)
            {
                var result = results.FirstOrDefault(r => r.order_id.Equals(payment.transaction_id));
                if (result == null) continue;
                result.num_of_payments++;
                result.num_of_paymentdistributions += distributions.Where(d => d.payment_id.Equals(payment.payment_id)).Count();
            }

            DeleteFileIfExists(helper.StatsFilePath);
            using var writer = File.CreateText(helper.StatsFilePath);

            var header = "order_id, num_of_ticket_sales, num_of_fee_sales, num_of_merch_sales, num_of_payments, num_of_paymentdistributions";
            writer.WriteLine(header);

            foreach (var result in results)
            {
                var resultString = $"{result.order_id}, {result.num_of_ticket_sales}, {result.num_of_fee_sales}, {result.num_of_merch_sales}, {result.num_of_payments}, {result.num_of_paymentdistributions}";
                writer.WriteLine(resultString);
            }
        }
        if (helper.extractOrderIds)
        {
            if (helper.IdArgs.Count == 0)
            {
                throw new Exception("No transaction id argument provided");
            }

            var salesResults = new List<Sale>();
            var paymentsResults = new List<Payment>();
            var distibutionsResults = new List<Distribution>();

            var sales = reader.ReadSales(helper.SalesFilePath);
            var payments = reader.ReadPayments(helper.PaymentFilePath);
            var distributions = reader.ReadDistributions(helper.DistributionFilePath);

            Console.WriteLine($"Total lines read:  Sales: {sales.Count} Payments: {payments.Count} Distibution: {distributions.Count}");

            foreach (var sale in sales)
            {
                if (helper.IdArgs.Exists(i => i.Equals(sale.transaction_id)))
                {
                    salesResults.Add(sale);
                }
            }

            foreach (var payment in payments)
            {
                if (helper.IdArgs.Contains(payment.transaction_id))
                {
                    paymentsResults.Add(payment);
                }
            }

            foreach (var distiribution in distributions)
            {
                if (paymentsResults.Exists(p => p.payment_id.Equals(distiribution.payment_id)))
                {
                    distibutionsResults.Add(distiribution);
                }
            }

            Console.WriteLine($"Total lines extracted:  Sales: {salesResults.Count} Payments: {paymentsResults.Count} Distibution: {distibutionsResults.Count}");

            Directory.CreateDirectory(Path.GetDirectoryName(helper.ExtractedSalesFilePath));
            Directory.CreateDirectory(Path.GetDirectoryName(helper.ExtractedPaymentsFilePath));
            Directory.CreateDirectory(Path.GetDirectoryName(helper.ExtractedDistributionsFilePath));

            DeleteFileIfExists(helper.ExtractedSalesFilePath);
            DeleteFileIfExists(helper.ExtractedPaymentsFilePath);
            DeleteFileIfExists(helper.ExtractedDistributionsFilePath);

            using var writerS = File.CreateText(helper.ExtractedSalesFilePath);
            foreach (var sale in salesResults)
            {
                writerS.WriteLine(sale.OriginalCsvLine);
            }

            using var writerP = File.CreateText(helper.ExtractedPaymentsFilePath);
            foreach (var payment in paymentsResults)
            {
                writerP.WriteLine(payment.OriginalCsvLine);
            }

            using var writerD = File.CreateText(helper.ExtractedDistributionsFilePath);
            foreach (var distribution in distibutionsResults)
            {
                writerP.WriteLine(distribution.OriginalCsvLine);
            }
        }

        Console.WriteLine($"Succesfuly executed. {watch.ElapsedMilliseconds} ms\n Press enter to exit ...");
        Console.ReadLine();
    }

    private static void DeleteFileIfExists(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
