using Auth.Jwt.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Jwt;

public class TokenUtil
{
    public static JwtSecurityToken GetToken(IOptions<JwtSettings> options, List<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
        var signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature);
        var expires = DateTime.UtcNow.AddMinutes(10);

        var token = new JwtSecurityToken(
            options.Value.Issuer,
            options.Value.Audience,
            claims,
            expires: expires,
            signingCredentials: signingCredentials);

        // new JwtSecurityTokenHandler().WriteToken(token)
        return token;
    }

    public static string GetToken(IOptions<JwtSettings> options, Func<IOptions<JwtSettings>, string> func)
    {
        return func.Invoke(options);
    }
}
