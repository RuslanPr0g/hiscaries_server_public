using HC.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace HC.Infrastructure.Configurations.Converters;

internal class IdentityConverter<TIdentity> : ValueConverter<TIdentity, Guid>
    where TIdentity : Identity
{
    internal IdentityConverter(Func<Guid, TIdentity> generator) : base(
        identity => identity.Value,
        value => generator.Invoke(value))
    {
    }
}
