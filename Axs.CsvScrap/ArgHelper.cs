namespace Axs.CsvScrap
{
    public class ArgHelper
    {
        public List<string> Files = new List<string>();
        public List<string> Fields = new List<string>();

        public bool isCallForHelp = false;
        public bool getUniqueTransactionIds = false;
        public bool returnAssosiatedOrders = false;

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
                }
                if (Path.Exists(arg))
                {
                    arg.Replace("\"", string.Empty);
                    Files.Add(arg);
                    continue;
                }
                else
                {
                    Fields.Add(arg);
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
