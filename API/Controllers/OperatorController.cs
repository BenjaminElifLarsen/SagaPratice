using API.Controllers.Extensions;
using Common.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleDomain.AL.Services.Operators;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class OperatorController : ControllerBase
{
    private readonly IOperatorService _operatorService;

	public OperatorController(IOperatorService operatorService)
	{
		_operatorService = operatorService;
	}

	[AllowAnonymous]
	[HttpGet]
	[Route("List")]
	public async Task<IActionResult> List()
	{
		var result = await _operatorService.GetOperatorListAsync();
		return this.FromResult(result);
	}

    [AllowAnonymous]
    [HttpGet]
    [Route("Details")]
    public async Task<IActionResult> Details([FromQuery] int id)
    {
        var result = await _operatorService.GetOperatorDetailsAsync(id);
        return this.FromResult(result);
    }

	[AllowAnonymous] //change later
	[HttpPost]
	[Route("System/Add")] //consider better name
	public async Task<IActionResult> Add([FromBody] AddOperatorNoLicenseFromSystem command)
	{
		var result = await _operatorService.AddOperatorFromSystemAsync(command);
		return this.FromResult(result);
	}

	[AllowAnonymous]
	[HttpPost]
	[Route("System/Delete")]
	public async Task<IActionResult> Remove([FromBody] RemoveOperatorFromSystem command)
	{
		var result = await _operatorService.RemoveOperatorFromSystemAsync(command);
		return this.FromResult(result);
	}
}
