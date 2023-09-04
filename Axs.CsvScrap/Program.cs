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
                var csvLines = reader.ReadWhereFieldEquals(file, 1, helper.Fields.First()).Result;
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

