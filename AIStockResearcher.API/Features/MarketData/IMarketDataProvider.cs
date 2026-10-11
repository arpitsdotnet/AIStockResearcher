namespace AIStockResearcher.API.Features.MarketData;

public interface IMarketDataProvider
{
    Task<StockQuote?> GetQuoteAsync(string symbol, CancellationToken cancellationToken);

    Task<IReadOnlyList<DailyPrice>> GetHistoryAsync(string symbol, DateOnly from, DateOnly to, CancellationToken cancellationToken);
}
