namespace SGP.Domain.ValueObjects;

public sealed record Email
{
    public Email(string address) => Address = address;

    private Email() // ORM
    {
    }

    public string Address
    {
        get;
        private init { field = value?.Trim().ToLowerInvariant(); }
    }

    public override string ToString() => Address;
}