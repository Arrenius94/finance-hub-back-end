using System.Security.Claims;
using FinanceHub.API.ExtensionsErrors;
using FinanceHub.Domain.DTOS.Input.Bill;
using FinanceHub.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHub.API.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]

public class BillController : ControllerBase
{
    private readonly IBillService _billService;
    
    public BillController(IBillService billService)
    {
        _billService = billService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBill([FromBody] CreateBillRequest billRequest,  CancellationToken ct)
    {
        var result = await _billService.CreateBillAsync(billRequest, ct);
        if (result.IsError)
            return result.ToActionResult();

        return Ok(new { id = result.Value });
    }

    [Authorize]
    [HttpGet("dashboard-metrics")]
    public async Task<IActionResult> GetDashboardMetrics(CancellationToken ct)
    {
        var result = await _billService.GetDashboardMetricsAsync(ct);
        if (result.IsError) 
            return result.ToActionResult();
        
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpGet("dashboard-graphics")]
    public async Task<IActionResult> GetDashboardGraphics([FromQuery] DashboarGraphicFilter filter, CancellationToken ct)
    {
        var result = await _billService.GetDashboardChartAsync(filter, ct);
        if (result.IsError)
            return result.ToActionResult();

        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpPost("payment-bills")]
    public async Task<IActionResult> PayBillList([FromBody] PayBillsListRequest request, CancellationToken ct)
    {
        var result = await _billService.PayBillListAsync(request, ct);
        if (result.IsError)
            return result.ToActionResult();

        return Ok();
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteBillList([FromBody] DeleteBillsRequest request, CancellationToken ct)
    {
        var result = await _billService.DeleteBillAsync(request, ct);
        if (result.IsError)
            return result.ToActionResult();

        return Ok();
    }
}