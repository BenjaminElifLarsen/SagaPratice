using Common.DDD;
using Common.Events.Domain;

namespace Security.DL.RefreshTokens;

public class RefreshToken : IAggregateRoot //does not need events for this, so consider splitting the aggregate root contact into two 
{ //so IAggregateRoot (wihtout events) and IEventAggregateRoot with only the event parts
    private int _refreshTokenId;
    private string _token;
    private bool _revoked;

    public int RefreshTokenId { get { return _refreshTokenId; } private set { _refreshTokenId = value; } }
    public string Token { get { return _token; } private set { _token = value; } }
    public bool Revoked { get { return _revoked; } private set { _revoked = value; } }

    public Guid Id => throw new NotImplementedException();

    public IEnumerable<DomainEvent> Events => throw new NotImplementedException();

    private RefreshToken()
    {

    }

    public RefreshToken(string token)
    {
        _token = token;
    }

    public void Revoke()
    {
        _revoked = true;
    }

    public void AddDomainEvent(DomainEvent eventItem)
    {
        throw new NotImplementedException();
    }

    public void RemoveDomainEvent(DomainEvent eventItem)
    {
        throw new NotImplementedException();
    }
}