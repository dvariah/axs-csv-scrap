namespace Axs.CsvScrap
{
    public class ArgHelper
    {
        public List<string> Files = new List<string>();
        public List<string> IdArgs = new List<string>();

        public string WorkFolderPath { get; set; }
        public string ArgumentFullPath { get; set; }

        public string SalesFilePath { get; set; }
        public string PaymentFilePath { get; set; }
        public string DistributionFilePath { get; set; }

        public string StatsFilePath { get; set; }
        public string ExtractedSalesFilePath { get; set; }
        public string ExtractedPaymentsFilePath { get; set; }
        public string ExtractedDistributionsFilePath { get; set; }

        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string Code { get; set; }

        public bool isCallForHelp = false;
        public bool getFileStats = false;
        public bool extractOrderIds = false;

        public static bool IsTraceEntryMode = false;

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
                    if (arg.Equals("--trace-method"))
                    {
                        IsTraceEntryMode = true;
                        continue;
                    }
                    Console.WriteLine("Unkown command");
                    isCallForHelp = true;
                    return;
                }
                if (TryGetFilePath(arg))
                {
                    WorkFolderPath = Path.GetDirectoryName(ArgumentFullPath);
                    var fileName = Path.GetFileName(ArgumentFullPath);
                    var n = fileName.Split("_");

                    CountryCode = n[n.Length - 3];
                    CityName = n[n.Length - 2];
                    Code = n[n.Length - 1].Replace(".csv.gz", string.Empty);

                    var firstPart = string.Empty;
                    for (int i = 0; i < n.Length - 3; i++)
                    {
                        firstPart += $"{n[i]}_";
                    }

                    SalesFilePath = $"{WorkFolderPath}\\{firstPart}{CountryCode}_{CityName}_{Code}.csv.gz";

                    firstPart = string.Empty;
                    for (int i = 0; i < n.Length - 3; i++)
                    {
                        if (n[i].Contains("sale"))
                        {
                            firstPart += "payment_";
                            continue;
                        }
                        firstPart += $"{n[i]}_";
                    }

                    PaymentFilePath = $"{WorkFolderPath}\\{firstPart}{CountryCode}_{CityName}_{Code}.csv.gz";

                    firstPart = string.Empty;
                    for (int i = 0; i < n.Length - 3; i++)
                    {
                        if (n[i].Contains("sale"))
                        {
                            firstPart += "payment_distribution_";
                            continue;
                        }
                        firstPart += $"{n[i]}_";
                    }

                    DistributionFilePath = $"{WorkFolderPath}\\{firstPart}{CountryCode}_{CityName}_{Code}.csv.gz";

                    StatsFilePath = SalesFilePath.Replace(".csv.gz", "-stats.csv");

                    ExtractedSalesFilePath = $"{WorkFolderPath}\\input\\sales\\{CountryCode}\\{CityName}\\axs_sales_{CountryCode}_{CityName}_{Code}.csv";
                    ExtractedPaymentsFilePath = $"{WorkFolderPath}\\input\\payment\\{CountryCode}\\{CityName}\\axs_payment_{CountryCode}_{CityName}_{Code}.csv";
                    ExtractedDistributionsFilePath = $"{WorkFolderPath}\\input\\payment_distribution\\{CountryCode}\\{CityName}\\axs_payment_distribution_{CountryCode}_{CityName}_{Code}.csv";

                    Console.WriteLine($"\nSalesFilePath : {SalesFilePath} \nPaymentFilePath : {PaymentFilePath} \nDistibutionFilePath: {DistributionFilePath}");
                    Console.WriteLine($"\nExtractedSalesFilePath : {ExtractedSalesFilePath}\n ExtractedPaymentsFilePath : {ExtractedPaymentsFilePath}\n ExtractedDistributionsFilePath: {ExtractedDistributionsFilePath}\n");
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

        public bool TryGetFilePath(string path)
        {
            var isFilePath = false;

            if (!(char.IsLetter(path[0]) && path[1] == ':'))
            {
                if (path.Contains(".."))
                {
                    var nodes = path.Split("\\");
                    var upCounter = 0;
                    string relativeFilePath = string.Empty;
                    foreach (var node in nodes)
                    {
                        if (node == "..")
                        {
                            upCounter++;
                        }
                        else
                        {
                            relativeFilePath = $"{relativeFilePath}\\{node}";
                        }
                    }
                    var currentFolder = Directory.GetCurrentDirectory();
                    var currentFolderNodes = currentFolder.Split("\\");
                    for (var i = 0; i < currentFolderNodes.Length - upCounter; i++)
                    {
                        ArgumentFullPath += currentFolderNodes[i] + "\\";
                    }
                    ArgumentFullPath = Path.TrimEndingDirectorySeparator(ArgumentFullPath) + relativeFilePath;
                }
                else
                {
                    ArgumentFullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
                }
            }
            else
            {
                ArgumentFullPath = path;
            }

            if (Path.Exists(ArgumentFullPath)) isFilePath = true;

            if (isFilePath && !path.Contains("csv.gz")) { throw new Exception("Not suported file format"); }

            return isFilePath;
        }

        public const string DOC = "Start with desired command followed by full path to csv file enclosed in quotes, follow by fields if needed\n" +
                                  "--command \"filePath\" order id\n" +
                                  "--help              -> shows generic help info\n" +
                                  "--get-file-stats    -> Reads csv archive and returns csv file with sale statistic per unique order\n" +
                                  "  e.g. ----extract-order-ids \"C:\\myfolder\\axs_sales_de_smalltown_1223456789.csv.gz\"\n" +
                                  "--extract-order-ids -> Reads three csv archives (sales, payments, payment distribution) and returns three csv files that contain only data assocciated with specified order id\n" +
                                  "  e.g. --orders-by-id \"C:\\myfolder\\axs_sales_de_smalltown_1223456789.csv.gz\" 1234988374\n";
    }
}
