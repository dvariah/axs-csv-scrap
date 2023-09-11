﻿namespace Axs.CsvScrap
{
    public class PathHelper
    {
        public static string CreateExtractedSalesFilePath(string workFolderPath, string countryCode, string cityName, string code)
        {
            return $"{workFolderPath}\\input\\sales\\{countryCode}\\{cityName}\\axs_sales_{countryCode}_{cityName}_{code}.csv";
        }

        public static string CreateExtractedPaymentsFilePath(string workFolderPath, string countryCode, string cityName, string code)
        {
            return $"{workFolderPath}\\input\\payment\\{countryCode}\\{cityName}\\axs_payment_{countryCode}_{cityName}_{code}.csv";
        }

        public static string CreateExtractedDistributionsFilePath(string workFolderPath, string countryCode, string cityName, string code)
        {
            return $"{workFolderPath}\\input\\payment_distribution\\{countryCode}\\{cityName}\\axs_payment_distribution_{countryCode}_{cityName}_{code}.csv";
        }
    }
}
