﻿using API.Controllers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleDomain.AL.Services.LicenseTypes;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;

namespace API.Controllers;
[Route("[controller]")]
[ApiController]
public class LicenseTypeController : ControllerBase
{
	private readonly ILicenseTypeService _licenseTypeService;
	public LicenseTypeController(ILicenseTypeService licenseTypeService)
	{
		_licenseTypeService = licenseTypeService;
	}

	[AllowAnonymous]
	[HttpGet]
	[Route("List")]
	public async Task<IActionResult> List()
	{
		var result = await _licenseTypeService.GetLicenseTypeListAsync();
		return this.FromResult(result);
	}

	[AllowAnonymous]
	[HttpGet]
	[Route("Details")]
	public async Task<IActionResult> Details([FromQuery] Guid id)
	{
		var result = await _licenseTypeService.GetLicenseTypeDetailsAsync(id);
		return this.FromResult(result);
	}

	[AllowAnonymous]
	[HttpPost]
	[Route("Add")]
	public async Task<IActionResult> Add([FromBody] EstablishLicenseTypeFromUser command)
	{
		var result = await _licenseTypeService.EstablishLicenseTypeAsync(command);
		return this.FromResult(result);
	}

	[AllowAnonymous]
	[HttpPost]
	[Route("Obsolete")]
	public async Task<IActionResult> Obsolete([FromBody] ObsoleteLicenseTypeFromUser command)
	{
		var result = await _licenseTypeService.ObsoleteLicenseTypeAsync(command);
		return this.FromResult(result);
	}

	[AllowAnonymous]
	[HttpPost]
	[Route("Alter")]
	public async Task<IActionResult> Alter([FromBody] AlterLicenseType command)
	{
		var result = await _licenseTypeService.AlterLicenseTypeAsync(command);
		return this.FromResult(result);
	}
}
