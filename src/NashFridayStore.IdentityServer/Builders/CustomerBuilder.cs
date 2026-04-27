using NashFridayStore.IdentityServer.Domain;

namespace NashFridayStore.IdentityServer.Builders;

public class CustomerBuilder : ApplicationUserBuilder
{
    private readonly Customer _customer = new();

    public CustomerBuilder()
    {
        _user = _customer;
    }

    public new Customer Build()
    {
        _customer.Id = Guid.NewGuid();
        return _customer;
    }
}
