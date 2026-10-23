using FinanceHub.API.ExtensionsErrors;
using FinanceHub.Domain.DTOS.Input.AccountReceivable;
using FinanceHub.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHub.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]

public class AccountReceivableController : ControllerBase
{
    public readonly IAccountReceivableService _accountReceivableService;

    public AccountReceivableController(IAccountReceivableService accountReceivableService)
    {
        _accountReceivableService = accountReceivableService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAccountReceivable([FromBody] CreateAccountReceivableRequest request, CancellationToken ct)
    {
        var result = await _accountReceivableService.CreateAccountReceivableServiceAsync(request, ct);
        if (result.IsError)
            return result.ToActionResult();

        return Ok(new { id = result.Value });
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
    {
        var result = await _accountReceivableService.GetAccountReceivableViewAsync(id, ct);
        if (result.IsError)
            return result.ToActionResult();

        return Ok(result.Value);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AccountReceivableFilter request, CancellationToken ct)
    {
        var result = await _accountReceivableService.GetAllReceivableViewAsync(request, ct);
        if (result.IsError)
            return result.ToActionResult();

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateAccountReceivable([FromRoute] int id, [FromBody] UpdateAccountReceivable request, CancellationToken ct)
    {
        var result = await _accountReceivableService.UpdateAccountReceivableAsync(request, id, ct);
        if (result.IsError)
            return result.ToActionResult();

        return NoContent();
    }

    [Authorize]
    [HttpPost("received-list")]
    public async Task<IActionResult> ReceivedList([FromBody] AccountReceivedListRequest request, CancellationToken ct)
    {
        var result = await _accountReceivableService.ReceivedListAsync(request, ct);
        if (result.IsError)
            return result.ToActionResult();
        return Ok();
    }

    [Authorize]
    [HttpGet("dashboard-metrics-receivable")]
    public async Task<IActionResult> GetDashboardMetricsReceivable(CancellationToken ct)
    {
        var result = await _accountReceivableService.GetThreeMetricsAsync(ct);
        if (result.IsError)
            return result.ToActionResult();
        return Ok(result.Value);
    }
    
    [Authorize]
    [HttpGet("dashboard-graphics-receivable")]
    public async Task<IActionResult> GetDashboardGraphicsReceivable([FromQuery] DashBoardGraphicFilter filter, CancellationToken ct)
    {
        var result = await _accountReceivableService.GetGraphicDataAsync(filter, ct);
        if (result.IsError)
            return result.ToActionResult();
        return Ok(result.Value);
    }

}