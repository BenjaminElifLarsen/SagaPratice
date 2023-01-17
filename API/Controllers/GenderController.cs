using API.Controllers.Extensions;
using Common.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonDomain.AL.Services.Genders;
using PersonDomain.DL.CQRS.Commands;

namespace API.Controllers;
[Route("[controller]")]
[ApiController]
public class GenderController : ControllerBase
{
	private IGenderService _genderService;

	public GenderController(IGenderService genderService)
	{
		_genderService = genderService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> List()
    {
        var result = await _genderService.GetGenderListAsync();
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route(nameof(GendersOverTime))]
    public async Task<IActionResult> GendersOverTime()
    {
        var result = await _genderService.GetGenderDataPointsOverTimeAsync();
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Details")]
    public async Task<IActionResult> Details([FromQuery] Guid id)
    {
        var result = await _genderService.GetGenderDetailsAsync(id);
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("Unrecognise")]
    public async Task<IActionResult> Unrecognise([FromBody] UnrecogniseGender command)
    {
        var result = await _genderService.UnrecogniseGenderAsync(command);
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("Recognise")]
    public async Task<IActionResult> Recognise([FromBody] RecogniseGender command)
    {
        var result = await _genderService.RecogniseGenderAsync(command);
        return this.FromResult(result);
    }
}
