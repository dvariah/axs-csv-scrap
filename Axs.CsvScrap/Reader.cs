namespace Axs.CsvScrap
{
    public class Reader : IReader
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

        /// <summary>
        /// Returns CSV element at index postion
        /// </summary>
        /// <param name="s">Input csv string</param>
        /// <param name="i">Index of looked forcsv element</param>
        /// <returns>CSV element at index postion</returns>
        /// <exception cref="Exception"></exception>
        public string GetField(string s, int i)
        {
            int start = 0;
            int end = 0;

            if (s == null || !s.Contains(Delimiter)) { throw new Exception("Not a csv line"); }

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
