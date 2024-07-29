using HC.Domain.Users;
using System;

namespace HC.Application.Services;

public sealed class RefreshTokenDescriptor
{
    public RefreshTokenDescriptor(
        string jwtId,
        string token,
        DateTime creationDate,
        DateTime expiryDate,
        bool used,
        bool invalidated)
    {
        JwtId = jwtId;
        Token = token;
        CreationDate = creationDate;
        ExpiryDate = expiryDate;
        Used = used;
        Invalidated = invalidated;
    }

    public string JwtId { get; init; }
    public string Token { get; init; }
    public DateTime CreationDate { get; init; }
    public DateTime ExpiryDate { get; init; }
    public bool Used { get; init; }
    public bool Invalidated { get; init; }

    public RefreshToken ToDomainModel(RefreshTokenId id) => new(
        id,
        Token,
        JwtId,
        CreationDate,
        ExpiryDate,
        Used,
        Invalidated);
}