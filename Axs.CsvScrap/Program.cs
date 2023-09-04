using Axs.CsvScrap;
using System.IO;

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

            if(!File.Exists(resultFilePath))
        {
                using (var writer = File.CreateText(resultFilePath))
                {
                   foreach (var row in result)
                    {
                        writer.WriteLine(row);
                    }
                }
            }
        }
    }
}

