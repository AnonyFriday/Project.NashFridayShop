using NashFridayStore.IdentityServer.Domain;

namespace NashFridayStore.IdentityServer.Builders;

public class AdminBuilder : ApplicationUserBuilder
{
    private readonly Admin _admin = new();

    public AdminBuilder()
    {
        _user = _admin;
    }

    public new Admin Build()
    {
        _admin.Id = Guid.NewGuid();
        return _admin;
    }
}
