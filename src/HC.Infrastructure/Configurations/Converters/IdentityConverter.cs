using HC.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace HC.Infrastructure.Configurations.Converters;

internal class IdentityConverter : ValueConverter<Identity, Guid>
{
    internal IdentityConverter() : base(
        identity => identity.Value,
        value => new Identity(value))
    {
    }
}
