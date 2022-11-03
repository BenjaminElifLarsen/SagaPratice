using API.Controllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleDomain.AL.Services.Vehicles;

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
    [Route("List")]
    public async Task<IActionResult> List()
    {
        var result = await _vehicleService.GetVehicleListAsync();
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Details")]
    public async Task<IActionResult> Details([FromQuery] int id)
    {
        var result = await _vehicleService.GetVehicleDetailsAsync(id);
        return this.FromResult(result);
    }
}
