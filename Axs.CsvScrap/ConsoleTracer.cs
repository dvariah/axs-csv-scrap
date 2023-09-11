﻿namespace Axs.CsvScrap
{
    public static class ConsoleTracer
    {
        public static void TraceMethodEntry(string message)
        {
            if (ArgHelper.IsTraceEntryMode)
            {
                Console.WriteLine(message);
            }
        }
    }
}
