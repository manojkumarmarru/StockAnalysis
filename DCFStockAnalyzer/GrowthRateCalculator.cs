public class GrowthRateCalculator
{
    public decimal CalculateAverageGrowthRate(List<HistoricalRevenue> historicalRevenue)
    {
        if (historicalRevenue == null || historicalRevenue.Count < 2)
            throw new ArgumentException("At least two years of data are required to calculate growth rate.");

        decimal totalGrowthRate = 0;
        int count = historicalRevenue.Count - 1;

        for (int i = 1; i < historicalRevenue.Count; i++)
        {
            decimal growthRate = (historicalRevenue[i].Revenue - historicalRevenue[i - 1].Revenue) / historicalRevenue[i - 1].Revenue;
            totalGrowthRate += growthRate;
        }

        return totalGrowthRate / count;
    }
}