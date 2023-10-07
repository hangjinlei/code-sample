﻿namespace Auth.Jwt.Models;

public class JwtSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Subject { get; set; } = default!;
}
