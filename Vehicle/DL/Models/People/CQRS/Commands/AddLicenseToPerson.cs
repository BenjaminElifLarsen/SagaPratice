﻿using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.People.CQRS.Commands;
internal class AddLicenseToPerson : ICommand
{
    public int PersonId { get; private set; }
    public DateTime Arquired { get; private set; }
    public int LicenseType { get; private set; }
}