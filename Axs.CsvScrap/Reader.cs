using System.IO.Compression;

namespace Axs.CsvScrap
{
    public class Reader
    {
        public string Delimiter { get { return ","; } }

        public async Task<List<string>> ReadWhereFieldEquals(string filePath, int idx, string fieldValue)
        {
            var result = new List<string>();
            using var reader = new StreamReader(filePath);

            var csvLine = await reader.ReadLineAsync();

            while (csvLine != null)
            {
                var field = GetField(csvLine, idx);
                if (field.Contains(fieldValue)) { result.Add(csvLine); }

                csvLine = await reader.ReadLineAsync();
            }

            return result;
        }

        public async Task<List<string>> ReadField(string filePath, int idx)
        {
            var result = new List<string>();
            using var reader = new StreamReader(filePath);

            var csvLine = await reader.ReadLineAsync();

            while (csvLine != null)
            {
                var field = GetField(csvLine, idx);
                if (!result.Contains(field)) { result.Add(field); }
                csvLine = await reader.ReadLineAsync();
            }

            return result;
        }

        public async Task<List<string>> ReadCsvLineWhereFieldEquals(string filePath, Dictionary<int, string> idxValuePair)
        {
            var result = new List<string>();
            using var fileStream = File.OpenRead(filePath);
            using var gzStream = new GZipStream(fileStream, CompressionMode.Decompress);
            using var reader = new StreamReader(gzStream);

            var csvLine = await reader.ReadLineAsync();

            while (csvLine != null)
            {
                var hit = true;

                foreach (var kvp in idxValuePair)
                {
                    var field = GetField(csvLine, kvp.Key);
                    if (field.Contains(kvp.Value)) { hit = false; break; }
                }

                if (hit) { result.Add(csvLine); }

                csvLine = await reader.ReadLineAsync();
            }

            return result;
        }

        public async Task<List<string>> ReadUniqueFieldFromCsvFile(string filePath, int idx)
        {
            var result = new List<string>();
            using var fileStream = File.OpenRead(filePath);
            using var gzStream = new GZipStream(fileStream, CompressionMode.Decompress);
            using var reader = new StreamReader(gzStream);

            var csvLine = await reader.ReadLineAsync();

            while (csvLine != null)
            {
                var field = GetField(csvLine, idx);
                if (!result.Contains(field)) { result.Add(field); }
                csvLine = await reader.ReadLineAsync();
            }

            return result;
        }

        public List<Sale> ReadSales(string filePath)
        {
            var result = new List<Sale>();
            using var fileStream = File.OpenRead(filePath);
            using var gzStream = new GZipStream(fileStream, CompressionMode.Decompress);
            using var reader = new StreamReader(gzStream);

            var csvLine = reader.ReadLine();

            while (csvLine != null)
            {
                var sale = new Sale();
                sale.unique_id = GetField(csvLine, 0);
                sale.transaction_id = GetField(csvLine, 1);
                sale.inventory_type = GetField(csvLine, 24);
                sale.OriginalCsvLine = csvLine;
                result.Add(sale);
                csvLine = reader.ReadLine();
            }

            return result;
        }

        public List<Payment> ReadPayments(string filePath)
        {
            var result = new List<Payment>();
            using var fileStream = File.OpenRead(filePath);
            using var gzStream = new GZipStream(fileStream, CompressionMode.Decompress);
            using var reader = new StreamReader(gzStream);

            var csvLine = reader.ReadLine();

            while (csvLine != null)
            {
                var payment = new Payment();
                payment.unique_id = GetField(csvLine, 0);
                payment.transaction_id = GetField(csvLine, 1);
                payment.payment_id = GetField(csvLine, 7);
                payment.OriginalCsvLine = csvLine;
                result.Add(payment);
                csvLine = reader.ReadLine();
            }

            return result;
        }

        public List<Distribution> ReadDistributions(string filePath)
        {
            var result = new List<Distribution>();
            using var fileStream = File.OpenRead(filePath);
            using var gzStream = new GZipStream(fileStream, CompressionMode.Decompress);
            using var reader = new StreamReader(gzStream);

            var csvLine = reader.ReadLine();

            while (csvLine != null)
            {
                var distribution = new Distribution();
                distribution.unique_id = GetField(csvLine, 0);
                distribution.payment_id = GetField(csvLine, 6);
                distribution.OriginalCsvLine = csvLine;
                result.Add(distribution);
                csvLine = reader.ReadLine();
            }

            return result;
        }

        /// <summary>
        /// Returns CSV element from CSV line at index postion
        /// </summary>
        /// <param name="s">Input csv string</param>
        /// <param name="i">Index of looked forcsv element</param>
        /// <returns>CSV element at index postion</returns>
        /// <exception cref="Exception"></exception>
        public string GetField(string s, int i)
        {
            int start = 0;
            int end = 0;

            if (s == null || !s.Contains(Delimiter))
            {
                Console.WriteLine("Error: Not a csv line\n Line: {s}");
                return null;
            }

            if (i == 0)
            {
                start = 0;
                end = s.IndexOf(Delimiter);
            }
            else
            {
                int numberOfOccurence = 0;
                int previous = 0;

                do
                {
                    int current = s.IndexOf(Delimiter, previous + 1);
                    numberOfOccurence++;
                    if (numberOfOccurence == i) { start = current + 1; }
                    if (numberOfOccurence == i + 1) { end = current; }
                    previous = current;
                }
                while (numberOfOccurence <= i + 1);
            }

            if (end == -1) { end = s.Length; }

            return s[start..end];
        }
    }
}
