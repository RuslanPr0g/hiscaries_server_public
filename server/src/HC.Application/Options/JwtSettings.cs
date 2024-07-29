using System;

namespace HC.Application.Options;

public class JwtSettings
{
    public string Key { get; set; }
    public TimeSpan TokenLifeTime { get; set; }
}