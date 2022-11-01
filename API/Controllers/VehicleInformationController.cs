using API.Controllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleDomain.AL.Services.VehicleInformations;

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
		var result = await _vehicleInformationService.GetVehicleInformationList();
		return this.FromResult(result);
	}
}
