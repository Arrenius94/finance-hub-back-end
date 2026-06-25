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
    public async Task<IActionResult> CreateBill([FromBody] CreateBill bill)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized("Usuário não identificado.");
        }

        int userId = int.Parse(userIdClaim);
        
        var result = await _billService.CreateBillAsync(bill, userId);
        if (result.IsError)
            return result.ToActionResult();

        return Ok(new { id = result.Value });
    }
}