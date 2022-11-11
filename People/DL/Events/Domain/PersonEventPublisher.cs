using PeopleDomain.DL.Model;

namespace PeopleDomain.DL.Events.Domain;
internal class PersonEventPublisher : IPersonEventPublisher
{
    public event IPersonEventPublisher.hireEventHandler RaiseHireEvent;
    public event IPersonEventPublisher.fireEventHandler RaiseFireEvent;
    public void Fire(Person person) //should person trigger the event? This would require breaking the purerity
    { //most likely best if the command handler generates the event and raises it.
        OnFire(new()); //the service cannot be called from the command handler as the service is a layer above the command handler
    }

    public void Hire(Person person)
    {
        OnHire(new(person));
    }

    public void OnFire(PersonFired e)
    {
        IPersonEventPublisher.fireEventHandler eventHandler = RaiseFireEvent;
        eventHandler?.Invoke(this, e);
    }

    public void OnHire(PersonHired e)
    {
        IPersonEventPublisher.hireEventHandler eventHandler = RaiseHireEvent;
        eventHandler?.Invoke(this, e);
    }
}
