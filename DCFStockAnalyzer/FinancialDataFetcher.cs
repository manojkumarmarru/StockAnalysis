using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FinancialDataFetcher
{
    private static readonly HttpClient client = new HttpClient();
    private string apiKey = "T3REvePztwVSl7HPnXrR9bBOErhAKROJ";  // Add your API key here

    public async Task<StockData> GetQuote(string symbol)
    {
        string url = $"https://financialmodelingprep.com/api/v3/quote/{symbol}?apikey={apiKey}";
        var response = await client.GetAsync(url);
        string responseBody = await response.Content.ReadAsStringAsync();
        var stockDataArray = JsonConvert.DeserializeObject<List<StockData>>(responseBody);
        return stockDataArray[0];  // Return the first item in the list
    }
    public async Task<BalanceSheetData> FetchBalanceSheetData(string symbol)
    {
        string url = $"https://financialmodelingprep.com/api/v3/balance-sheet-statement/{symbol}?apikey={apiKey}";
        var response = await client.GetStringAsync(url);
        var balanceSheets = JsonConvert.DeserializeObject<List<BalanceSheetData>>(response);
        return balanceSheets[0];  // Return the latest balance sheet data
    }

    public async Task<IncomeStatementData> FetchIncomeStatementData(string symbol)
    {
        string url = $"https://financialmodelingprep.com/api/v3/income-statement/{symbol}?apikey={apiKey}";
        var response = await client.GetStringAsync(url);
        var incomeStatements = JsonConvert.DeserializeObject<List<IncomeStatementData>>(response);
        return incomeStatements[0];  // Return the latest income statement data
    }

    public async Task<List<HistoricalRevenue>> FetchHistoricalRevenue(string symbol)
    {
        string url = $"https://financialmodelingprep.com/api/v3/income-statement/{symbol}?apikey={apiKey}";
        var response = await client.GetAsync(url);
        string responseBody = await response.Content.ReadAsStringAsync();
        var incomeStatements = JsonConvert.DeserializeObject<List<IncomeStatementData>>(responseBody);

        var historicalRevenue = new List<HistoricalRevenue>();
        foreach (var statement in incomeStatements)
        {
            historicalRevenue.Add(new HistoricalRevenue
            {
                Year = statement.date.Year,
                Revenue = statement.Revenue
            });
        }
        return historicalRevenue;
    }

    public async Task<WACCData> FetchWACCData(string symbol)
    {
        // Fetch market capitalization
        var stockData = await GetQuote(symbol);
        decimal marketValueOfEquity = stockData.MarketCap;

        // Fetch balance sheet data
        var balanceSheet = await FetchBalanceSheetData(symbol);
        decimal marketValueOfDebt = balanceSheet.totalDebt;

        // Fetch income statement data to get the tax rate
        var incomeStatement = await FetchIncomeStatementData(symbol);
        decimal corporateTaxRate = CalculateTaxRate(incomeStatement);

        // Fetch key metrics data
        string url = $"https://financialmodelingprep.com/api/v3/company-key-metrics/{symbol}?apikey={apiKey}";
        var response = await client.GetStringAsync(url);
        var keyMetrics = JsonConvert.DeserializeObject<KeyMetricsData>(response);

        // Calculate Cost of Equity using CAPM
        decimal costOfEquity = keyMetrics.RiskFreeRate + keyMetrics.Beta * (keyMetrics.MarketReturn - keyMetrics.RiskFreeRate);

        // Calculate Cost of Debt
        decimal costOfDebt = keyMetrics.InterestCoverage != 0 ? 1 / keyMetrics.InterestCoverage : 0;

        return new WACCData
        {
            MarketValueOfEquity = marketValueOfEquity,
            MarketValueOfDebt = marketValueOfDebt,
            CostOfEquity = costOfEquity,
            CostOfDebt = costOfDebt,
            CorporateTaxRate = corporateTaxRate
        };
    }

    private decimal CalculateTaxRate(IncomeStatementData incomeStatement)
    {
        if (incomeStatement.IncomeBeforeTax == 0)
        {
            return 0;
        }
        return incomeStatement.IncomeTaxExpense / incomeStatement.IncomeBeforeTax;
    }
}
