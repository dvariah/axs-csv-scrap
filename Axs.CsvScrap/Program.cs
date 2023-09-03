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
        if (helper.getUniqueTransactionIds)
        {
            var result = new List<string>();
            foreach (var file in helper.Files)
            {
                var fields = reader.ReadField(file, 2).Result;
                result.AddRange(fields);
            }
            foreach (var r in result)
            {
                Console.WriteLine(r);
            }
        }
        if (helper.returnAssosiatedOrders)
        {
            var result = new List<string>();
            foreach (var file in helper.Files)
            {
                var csvLines = reader.ReadWhereFieldEquals(file, 2, helper.Fields.First()).Result;
                result.AddRange(csvLines);
                Console.WriteLine($"here {csvLines.Count}");
            }
            foreach (var r in result)
            {
                Console.WriteLine(r);
            }
        }
    }
}

