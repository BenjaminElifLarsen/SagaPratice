using API.Controllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleDomain.AL.Services;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

	public VehicleController(IVehicleService vehicleService)
	{
		_vehicleService = vehicleService;
	}

	[AllowAnonymous]
	[HttpGet]
	[Route("OperatorList")]
	public async Task<IActionResult> List()
	{
		var result = await _vehicleService.GetOperatorListAsync();
		return this.FromResult(result);
	}
}
