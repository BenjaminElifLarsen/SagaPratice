using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class AddOperatorNoLicenseFromSystem : ICommand
{ //rename this command
    public Guid Id { get;  set; }
    public DateTime Birth { get;  set; }

    public Guid CommandId => throw new NotImplementedException();

    public Guid CorrelationId => throw new NotImplementedException();

    public Guid CausationId => throw new NotImplementedException();
}