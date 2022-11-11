using Common.Events.Domain;
using PeopleDomain.DL.Model;

namespace PeopleDomain.DL.Events.Domain;
public interface IPersonEventPublisher
{
    public delegate void hireEventHandler(object sender, PersonHired args);
    public event hireEventHandler RaiseHireEvent;
    public void Hire(Person person);
    public void OnHire(PersonHired e);

    public delegate void fireEventHandler(object sender, PersonFired args);
    public event fireEventHandler RaiseFireEvent;
    public void Fire(Person person);
    public void OnFire(PersonFired e);
}
