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
            GetFileStats(reader, helper);
        }
        if (helper.extractOrderIds)
        {
            ExtractOrderIds(reader, helper);
        }

        Console.WriteLine($"Succesfuly executed. {watch.ElapsedMilliseconds} ms\n Press enter to exit ...");
        Console.ReadLine();
    }

    private static void GetFileStats(Reader reader, ArgHelper helper)
    {
        ConsoleTracer.TraceMethodEntry($"Enter GetFileStats method.");

        var results = new List<SaleStats>();
        var sales = reader.ReadSales(helper.SalesFilePath);
        var payments = reader.ReadPayments(helper.PaymentFilePath);
        var distributions = reader.ReadDistributions(helper.DistributionFilePath);

        foreach (var sale in sales)
        {
            var stats = results.FirstOrDefault(r => r.order_id.Equals(sale.transaction_id));
            if (stats == null)
            {
                stats = new SaleStats
                {
                    order_id = sale.transaction_id,
                    venue = sale.venue,
                    client = sale.client,
                    event_name = sale.event_name,
                    zone_type = sale.zone_type,
                    price_code_type = sale.price_code_type,
                };
                results.Add(stats);
            }

            switch (sale.inventory_type)
            {
                case "\"0\"":
                    stats.num_of_fee_sales++;
                    break;
                case "\"1\"":
                    stats.num_of_merch_sales++;
                    break;
                case "\"2\"":
                    stats.num_of_ticket_sales++;
                    break;
            }
        }

        foreach (var payment in payments)
        {
            var result = results.FirstOrDefault(r => r.order_id.Equals(payment.transaction_id));
            if (result == null) continue;

            result.merch_id = payment.merch_id;
            result.outlet = payment.outlet;
            result.payment_type = payment.payment_type;

            result.num_of_payments++;
            result.num_of_paymentdistributions += distributions.Where(d => d.payment_id.Equals(payment.payment_id)).Count();
        }

        ResultFileWriter.WriteStatsFile(helper.StatsFilePath, results);

        Console.WriteLine($"Stats file path: {helper.StatsFilePath}");

        ConsoleTracer.TraceMethodEntry($"Enter GetFileStats method.");
    }

    private static void ExtractOrderIds(Reader reader, ArgHelper helper)
    {
        ConsoleTracer.TraceMethodEntry($"Enter ExtractOrderIds method.");

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

        decimal salesAmount = 0;
        decimal paymentsAmount = 0;
        decimal distirbutionsAmount = 0;

        Console.WriteLine($"Total lines read:  Sales: {sales.Count} Payments: {payments.Count} Distibution: {distributions.Count}");

        foreach (var sale in sales)
        {
            if (helper.IdArgs.Exists(i => i.Equals(sale.transaction_id)))
            {
                salesResults.Add(sale);
                salesAmount += sale.total_sales_gross_amount;
            }
        }

        foreach (var payment in payments)
        {
            if (helper.IdArgs.Contains(payment.transaction_id))
            {
                paymentsResults.Add(payment);
                paymentsAmount += payment.payment_amount;
            }
        }

        foreach (var distiribution in distributions)
        {
            if (paymentsResults.Exists(p => p.payment_id.Equals(distiribution.payment_id)))
            {
                distibutionsResults.Add(distiribution);
                distirbutionsAmount += distiribution.distribution_amount;
            }
        }

        Console.WriteLine($"Total lines extracted:  Sales: {salesResults.Count} Payments: {paymentsResults.Count} Distibution: {distibutionsResults.Count}");
        Console.WriteLine($"Failed to parse {reader.IncorrectCvsLineErrorCounter} lines (incorrect csv line). Failed to parse {reader.NotANumberErrorCounter} number. (not a number).");

        ResultFileWriter.WriteExtractedSalesFile(helper.ExtractedSalesFilePath, salesResults);
        ResultFileWriter.WriteExtractedPamentsFile(helper.ExtractedPaymentsFilePath, paymentsResults);
        ResultFileWriter.WriteExtractedDistributionFile(helper.ExtractedDistributionsFilePath, distibutionsResults);

        ResultFileWriter.WriteControlCard(helper.ControlCardSalesFilePath, $"{salesResults.Count}, {salesAmount.ToString()}");
        ResultFileWriter.WriteControlCard(helper.ControlCardPaymentsFilePath, $"{paymentsResults.Count}, {paymentsAmount.ToString()}");
        ResultFileWriter.WriteControlCard(helper.ControlCardDistributionsFilePath, $"{distibutionsResults.Count}, {distirbutionsAmount.ToString()}");

        ConsoleTracer.TraceMethodEntry($"Exit ExtractOrderIds method.");
    }

    private static void DeleteFileIfExists(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
