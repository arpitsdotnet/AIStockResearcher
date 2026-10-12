
namespace AIStockResearcher.API.Schedulers;

public class DailyMarketReportWorker : BackgroundService
{
    private readonly ILogger<DailyMarketReportWorker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DailyMarketReportWorker(
        ILogger<DailyMarketReportWorker> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTimeOffset.UtcNow;

            var indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
                OperatingSystem.IsWindows()
                ? "India Standard Time"
                : "Asia/Kolkata");

            var indiaNow = TimeZoneInfo.ConvertTime(now, indiaTimeZone);

            var runAt = new DateTime(
                indiaNow.Year,
                indiaNow.Month,
                indiaNow.Day,
                16, 30, 0, DateTimeKind.Unspecified); // 4:30 PM IST

            if (indiaNow.TimeOfDay >= TimeSpan.FromHours(16.5))
            {
                runAt = runAt.AddDays(1); // Schedule for the next day if it's past 4:30 PM IST
            }

            var nextRun = new DateTimeOffset(
                runAt, indiaTimeZone.GetUtcOffset(runAt));

            await Task.Delay(nextRun - now, stoppingToken);

            try
            {

            }
            catch (OperationCanceledException ex)
            {
                _logger.LogInformation("Watchlist and portfolio price refresh operation was canceled.");
                break; // Exit the loop if the operation was canceled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing watchlist and portfolio prices.");
            }

            Console.WriteLine($"Refreshing daily market report at {DateTime.UtcNow}");
            // Wait for 5 minutes before the next refresh
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}