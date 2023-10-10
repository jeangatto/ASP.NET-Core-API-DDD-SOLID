namespace SGP.Domain.ValueObjects;

public sealed record Email
{
    private readonly string _emailAddress;

    public Email(string address) => Address = address;

    private Email() // ORM
    {
    }

    public string Address
    {
        get { return _emailAddress; }
        private init { _emailAddress = value?.Trim().ToLowerInvariant(); }
    }

    public override string ToString() => Address;
}