using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIStockResearcher.API.Controllers
{
    [Route("api/market")]
    [ApiController]
    public class MarketManagementController : ControllerBase
    {
        [HttpGet("overview")]
        public IActionResult GetMarketOverview()
        {
            var marketStatus = new
            {
                Status = "Open",
                Time = DateTime.UtcNow
            };
            return Ok(marketStatus);
        }

        [HttpGet("daily-report")]
        public IActionResult GetDailyReport()
        {
            var dailyReport = new
            {
                Date = DateTime.UtcNow.Date,
                Summary = "Market is performing well today."
            };
            return Ok(dailyReport);
        }
    }
}
