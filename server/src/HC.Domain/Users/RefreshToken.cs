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
        Refresh(token);
    }

    public string JwtId { get; private set; }
    public string Token { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public bool Used { get; private set; }
    public bool Invalidated { get; private set; }

    internal void Refresh(RefreshToken token)
    {
        JwtId = token.JwtId;
        Token = token.Token;
        CreationDate = token.CreationDate;
        ExpiryDate = token.ExpiryDate;
        Used = token.Used;
        Invalidated = token.Invalidated;
    }

    internal bool Validate()
    {
        if (DateTime.UtcNow > ExpiryDate || Invalidated || Used)
        {
            return false;
        }

        Used = true;

        return true;
    }

    private RefreshToken()
    {
    }
}