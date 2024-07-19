using System;

namespace HC.Domain.Users;

public sealed class RefreshToken : Entity<RefreshTokenId>
{
    public RefreshToken(
        RefreshTokenId id,
        string jwtId,
        string token,
        DateTime creationDate,
        DateTime expiryDate,
        bool used,
        bool invalidated) : base(id)
    {
        JwtId = jwtId;
        Token = token;
        CreationDate = creationDate;
        ExpiryDate = expiryDate;
        Used = used;
        Invalidated = invalidated;
    }

    public RefreshToken(RefreshToken token) : base(token.Id)
    {
        JwtId = token.JwtId;
        Token = token.Token;
        CreationDate = token.CreationDate;
        ExpiryDate = token.ExpiryDate;
        Used = token.Used;
        Invalidated = token.Invalidated;
    }

    public string JwtId { get; init; }
    public string Token { get; init; }
    public DateTime CreationDate { get; init; }
    public DateTime ExpiryDate { get; init; }
    public bool Used { get; init; }
    public bool Invalidated { get; init; }

    internal void Refresh(RefreshToken refreshToken)
    {
        throw new NotImplementedException();
    }

    private RefreshToken()
    {
    }
}