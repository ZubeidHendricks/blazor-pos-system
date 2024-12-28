using Microsoft.AspNetCore.Authorization;

namespace BlazorPOS.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet("sales")]
        [Authorize(Policy = "ViewReports")]
        public async Task<ActionResult<SalesSummaryReport>> GetSalesSummary(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            var report = await _reportingService.GetSalesSummaryAsync(startDate, endDate);
            return Ok(report);
        }

        [HttpGet("inventory")]
        [Authorize(Policy = "ViewInventory")]
        public async Task<ActionResult<InventoryReport>> GetInventoryReport()
        {
            var report = await _reportingService.GetInventoryReportAsync();
            return Ok(report);
        }
    }
}