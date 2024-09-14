using System;
using System.Threading.Tasks;

namespace DCFStockAnalyzer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a stock symbol as an argument.");
                return;
            }

            string symbol = args[0];

            try
            {
                StockAnalysis stockAnalysis = new StockAnalysis();
                await stockAnalysis.AnalyzeStock(symbol);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}