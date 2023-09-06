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

        if (helper.getUniqueTransactionIds)
        {
            var result = new List<string>();
            foreach (var file in helper.Files)
            {
                var fields = reader.ReadField(file, 1).Result;
                result.AddRange(fields);
            }
        }
        if (helper.returnAssosiatedOrders)
        {
            var result = new List<string>();
            foreach (var file in helper.Files)
            {
                var csvLines = reader.ReadWhereFieldEquals(file, 1, helper.IdArgs.First()).Result;
                result.AddRange(csvLines);
            }

            var firstFilePath = helper.Files.First();
            var resultFilePath = firstFilePath.Replace(Path.GetFileNameWithoutExtension(firstFilePath), "Results");

            DeleteFileIfExists(resultFilePath);

            using (var writer = File.CreateText(resultFilePath))
            {
                foreach (var row in result)
                {
                    writer.WriteLine(row);
                }
            }
        }
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

            var resultFilePath = helper.SalesFilePath.Replace(".csv.gz", "-stats.csv");
            DeleteFileIfExists(resultFilePath);
            using var writer = File.CreateText(resultFilePath);

            foreach (var result in results)
            {
                var resultString = $"{result.order_id}, {result.num_of_ticket_sales}, {result.num_of_fee_sales}, {result.num_of_merch_sales}, {result.num_of_payments}, {result.num_of_paymentdistributions}";
                Console.WriteLine(resultString);
                writer.WriteLine(resultString);
            }
        }
        if (helper.extractOrderIds)
        {
            var salesResults = new List<Sale>();
            var paymentsResults = new List<Payment>();
            var distibutionsResults = new List<Distribution>();

            var sales = reader.ReadSales(helper.SalesFilePath);
            var payments = reader.ReadPayments(helper.PaymentFilePath);
            var distributions = reader.ReadDistributions(helper.DistributionFilePath);

            foreach (var sale in sales)
            {
                if (helper.IdArgs.Contains(sale.transaction_id))
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

            var resultSalesFilePath = helper.SalesFilePath.Replace(".csv.gz", "-extracted.csv");
            var resultPaymentsFilePath = helper.PaymentFilePath.Replace(".csv.gz", "-extracted.csv");
            var resultDistributionsFilePath = helper.DistributionFilePath.Replace(".csv.gz", "-extracted.csv");

            DeleteFileIfExists(resultSalesFilePath);
            DeleteFileIfExists(resultPaymentsFilePath);
            DeleteFileIfExists(resultDistributionsFilePath);

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

