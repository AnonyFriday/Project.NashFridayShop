using NashFridayStore.IdentityServer.Domain;

namespace NashFridayStore.IdentityServer.Builders;

public class ApplicationUserBuilder
{
    protected ApplicationUser _user = new();

    public ApplicationUserBuilder WithAddress(string address)
    {
        _user.Address = address;
        return this;
    }

    public ApplicationUserBuilder WithFullName(string fullName)
    {
        _user.FullName = fullName;
        return this;
    }

    public ApplicationUserBuilder WithEmail(string email)
    {
        _user.Email = email;
        _user.UserName = email;
        _user.NormalizedEmail = email.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
        _user.NormalizedUserName = email.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
        return this;
    }

    public ApplicationUserBuilder WithUserName(string userName)
    {
        _user.UserName = userName;
        _user.NormalizedUserName = userName.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
        return this;
    }

    public ApplicationUserBuilder WithPhone(string phone)
    {
        _user.PhoneNumber = phone;
        return this;
    }

    public ApplicationUserBuilder WithCreatedAt(DateTime? createdAt = null)
    {
        _user.CreatedAtUtc = createdAt ?? DateTime.UtcNow;
        return this;
    }

    public ApplicationUserBuilder WithUpdatedAt(DateTime? updatedAt = null)
    {
        _user.UpdatedAtUtc = updatedAt ?? DateTime.UtcNow;
        return this;
    }

    public ApplicationUserBuilder WithIsDeleted()
    {
        _user.IsDeleted = true;
        _user.DeletedAtUtc = DateTime.UtcNow;
        return this;
    }

    public ApplicationUser Build()
    {
        _user.Id = Guid.NewGuid();
        return _user;
    }
}
