public class DCFCalculator
{
    public decimal CalculateDCF(StockData stockData, BalanceSheetData balanceSheet, IncomeStatementData incomeStatement, WACCData waccData, decimal growthRate)
    {
        // Our API DCF calculation:
        
        //Market Cap = Weigted Average Shares Outstanding Diluted * Stock Price
        var marketCap = incomeStatement.WeightedAverageShsOutDil * stockData.Price;

        // Enterprise Value NB = Market Cap + Long Term Debt + Short Term Debt
        decimal enterpriseValue = marketCap + balanceSheet.longTermDebt + balanceSheet.shortTermDebt;

        //Equity Value = Enterprise Value NB - Net Debt
        decimal equityValue = enterpriseValue - balanceSheet.netDebt;

        //DCF = Equity Value / Weigted Average Shares Outstanding Diluted
        decimal dcf = equityValue / incomeStatement.WeightedAverageShsOutDil;

        // Adjust DCF with growth rate (simplified example)
        dcf *= (1 + growthRate);
        
        return dcf;
    }
}