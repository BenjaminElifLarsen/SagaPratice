using API.Controllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonDomain.AL.Services.People;
using PersonDomain.DL.CQRS.Commands;

namespace API.Controllers;
[Route("[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService _peopleService;

	public PersonController(IPersonService peopleService)
	{
		_peopleService = peopleService;
    }


    [AllowAnonymous]
    [HttpGet]
    [Route(nameof(List))]
    public async Task<IActionResult> List()
    {
        var result = await _peopleService.GetPeopleListAsync();
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route(nameof(GenderChangesOverTime))]
    public async Task<IActionResult> GenderChangesOverTime()
    { //consider updating to be person changes over time and have first and last name
        var result = await _peopleService.GetGendersOverTimeAsync();
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route(nameof(Details))]
    public async Task<IActionResult> Details([FromQuery] Guid id)
    {
        var result = await _peopleService.GetPersonDetailsAsync(id);
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route(nameof(Hire))]
    public async Task<IActionResult> Hire([FromBody] HirePersonFromUser command)
    {
        var result = await _peopleService.HirePersonAsync(command);
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route(nameof(Fire))]
    public async Task<IActionResult> Fire([FromBody] FirePersonFromUser command)
    {
        var result = await _peopleService.FirePersonAsync(command);
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> ChangePersonalInformation([FromBody] ChangePersonalInformationFromUser command)
    {
        var result = await _peopleService.ChangePersonalInformationAsync(command);
        return this.FromResult(result);
    }

}
