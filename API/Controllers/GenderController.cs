using API.Controllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleDomain.AL.Services.Genders;

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
    [Route("Details")]
    public async Task<IActionResult> Details([FromQuery] int id)
    {
        var result = await _genderService.GetGenderDetailsAsync(id);
        return this.FromResult(result);
    }
}
