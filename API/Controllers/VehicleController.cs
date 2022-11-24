using API.Controllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleDomain.AL.Services.Vehicles;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

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
    [Route("List/Active")]
    public async Task<IActionResult> ListActive()
    {
        var result = await _vehicleService.GetVehicleInUseListAsync();
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

    [AllowAnonymous]
    [HttpPost]
    [Route("Start")]
    public async Task<IActionResult> StartOperating([FromBody]StartOperatingVehicle command)
    {
        var result = await _vehicleService.StartOperatingVehicleAsync(command);
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("Stop")]
    public async Task<IActionResult> StopOperating([FromBody] StopOperatingVehicle command)
    {
        var result = await _vehicleService.StopOperatingVehicleAsync(command);
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("Buy")]
    public async Task<IActionResult> BuyVehicleNoOperator([FromBody]BuyVehicleWithNoOperator command)
    {
        var result = await _vehicleService.BuyVehicleNoOperatorAsync(command);
        return this.FromResult(result);
    }

    //[AllowAnonymous]
    //[HttpPost]
    //[Route("Buy/WithOperator")]
    //public async Task<IActionResult> BuyVehicleWithOperator([FromBody] BuyVehicleWithOperators command)
    //{
    //    var result = await _vehicleService.BuyVehicleWithOperator(command);
    //    return this.FromResult(result);
    //}
}
