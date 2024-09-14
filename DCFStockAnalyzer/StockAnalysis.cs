public class StockAnalysis
{
    public async Task AnalyzeStock(string symbol)
    {
        FinancialDataFetcher fetcher = new FinancialDataFetcher();
        GrowthRateCalculator growthRateCalculator = new GrowthRateCalculator();

        var stockData = await fetcher.GetQuote(symbol);
        var balanceSheet = await fetcher.FetchBalanceSheetData(symbol);
        var incomeStatement = await fetcher.FetchIncomeStatementData(symbol);
        var waccData = await fetcher.FetchWACCData(symbol);
        var historicalRevenue = await fetcher.FetchHistoricalRevenue(symbol);
        decimal growthRate = growthRateCalculator.CalculateAverageGrowthRate(historicalRevenue);

        DCFCalculator calculator = new DCFCalculator();
        decimal dcfValue = calculator.CalculateDCF(stockData, balanceSheet, incomeStatement, waccData, growthRate);

        Console.WriteLine($"The StockData for {symbol} is Price: {stockData.Price}, MarketCap: {stockData.MarketCap}");
        Console.WriteLine($"The BalanceSheet for {symbol} is longTermDebt: {balanceSheet.longTermDebt}, shortTermDebt: {balanceSheet.shortTermDebt}, TotalDebt: {balanceSheet.totalDebt}, NetDebt: {balanceSheet.netDebt}" );
        Console.WriteLine($"The IncomeStatement for {symbol} is WeightedAverageShsOutDil: {incomeStatement.WeightedAverageShsOutDil}");
        Console.WriteLine($"The WACC for {symbol} is MarketValueOfEquity: {waccData.MarketValueOfEquity}, MarketValueOfDebt: {waccData.MarketValueOfDebt}");
        Console.WriteLine($"The GrowthRate for {symbol} is {growthRate}");
        Console.WriteLine($"The HistoricalRevenue for {symbol} is {historicalRevenue[0].Year}: {historicalRevenue[0].Revenue}");
        Console.WriteLine($"The DCF for {symbol} is {dcfValue}");
    }
}