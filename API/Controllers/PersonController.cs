using API.Controllers.Extensions;
using Common.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleDomain.AL.Services.People;
using PeopleDomain.DL.CQRS.Commands;

namespace API.Controllers;
[Route("[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPeopleService _peopleService;

	public PersonController(IPeopleService peopleService, IRoutingRegistry registry)
	{
		_peopleService = peopleService;
        registry.SetUpRouting();
    }


    [AllowAnonymous]
    [HttpGet]
    [Route("List")]
    public async Task<IActionResult> List()
    {
        var result = await _peopleService.GetPeopleListAsync();
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Details")]
    public async Task<IActionResult> Details([FromQuery] int id)
    {
        var result = await _peopleService.GetPersonDetailsAsync(id);
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("Hire")]
    public async Task<IActionResult> Hire([FromBody] HirePersonFromUser command)
    {
        var result = await _peopleService.HirePersonAsync(command);
        return this.FromResult(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("Fire")]
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
