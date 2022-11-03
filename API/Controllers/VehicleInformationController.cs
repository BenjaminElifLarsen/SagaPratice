using API.Controllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleDomain.AL.Services.VehicleInformations;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleInformationController : ControllerBase
{
	private readonly IVehicleInformationService _vehicleInformationService;
	public VehicleInformationController(IVehicleInformationService vehicleInformationService)
	{
		_vehicleInformationService = vehicleInformationService;
	}

	[AllowAnonymous]
	[HttpGet]
	[Route("List")]
	public async Task<IActionResult> List()
	{
		var result = await _vehicleInformationService.GetVehicleInformationListAsync();
		return this.FromResult(result);
	}

    [AllowAnonymous]
    [HttpGet]
    [Route("Details")]
    public async Task<IActionResult> Details([FromQuery] int id)
    {
        var result = await _vehicleInformationService.GetVehicleInformationDetailsAsync(id);
        return this.FromResult(result);
    }

	[AllowAnonymous]
	[HttpPost]
	[Route("System/Add")]
	public async Task<IActionResult> Add([FromBody] AddVehicleInformationFromSystem command)
	{
		var result = await _vehicleInformationService.SetupVehicleInformation(command);
		return this.FromResult(result);
	}
}
