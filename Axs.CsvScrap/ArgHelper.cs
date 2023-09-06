namespace Axs.CsvScrap
{
    public class ArgHelper
    {
        public List<string> Files = new List<string>();
        public List<string> IdArgs = new List<string>();

        public string WorkFolderPath { get; set; }
        public string SalesFilePath { get; set; }
        public string PaymentFilePath { get; set; }
        public string DistributionFilePath { get; set; }

        public bool isCallForHelp = false;
        public bool getUniqueTransactionIds = false;
        public bool returnAssosiatedOrders = false;

        public bool getFileStats = false;
        public bool extractOrderIds = false;

        public void Parse(string[] Args)
        {
            if (Args.Length <= 0)
            {
                isCallForHelp = true;
                return;
            }

            foreach (var arg in Args)
            {
                if (arg.StartsWith("--"))
                {
                    if (arg.Equals("--help"))
                    {
                        isCallForHelp = true;
                        return;
                    }
                    if (arg.Equals("--unique-transactions"))
                    {
                        getUniqueTransactionIds = true;
                        continue;
                    }
                    if (arg.Equals("--orders-by-id"))
                    {
                        returnAssosiatedOrders = true;
                        continue;
                    }
                    if (arg.Equals("--extract-order-ids"))
                    {
                        extractOrderIds = true;
                        continue;
                    }
                    if (arg.Equals("--get-file-stats"))
                    {
                        getFileStats = true;
                        continue;
                    }
                }
                if (Path.Exists(arg))
                {
                    if (!arg.Contains("csv.gz")) { throw new Exception("Not suported file format"); }
                    WorkFolderPath = Path.GetDirectoryName(arg);

                    //get file name nodes
                    var n = arg.Split("_");
                    SalesFilePath = $"{WorkFolderPath}\\axs_sales_{n[n.Length - 3]}_{n[n.Length - 2]}_{n[n.Length - 1]}";
                    PaymentFilePath = $"{WorkFolderPath}\\axs_payment_{n[n.Length - 3]}_{n[n.Length - 2]}_{n[n.Length - 1]}";
                    DistributionFilePath = $"{WorkFolderPath}\\axs_payment_distribution_{n[n.Length - 3]}_{n[n.Length - 2]}_{n[n.Length - 1]}";

                    Console.WriteLine($"SalesFilePath : {SalesFilePath}, PaymentFilePath : {PaymentFilePath}, DistibutionFilePath: {DistributionFilePath}");
                    continue;
                }
                else
                {
                    var id = $"\"{arg}\"";
                    IdArgs.Add(id);
                    continue;
                }
            }
        }

        public const string DOC = "Start with desired command followed by full path to csv file enclosed in quotes, follow by fields if needed\n" +
                                  "--command \"filePath\" field\n" +
                                  "--help                -> shows generic help info\n" +
                                  "--unique-transactions -> returns a collection of unique transaction ids from csv file\n" +
                                  "  e.g. --unique-tansactions \"C:\\newfolder\\sales.csv\"\n" +
                                  "--orders-by-id        -> creates a csv file with orders that have specified transaction id/n" +
                                  "  e.g. --orders-by-id \"C:\\myfolder\\sales.csv\" 1234988374\n";
    }
}
