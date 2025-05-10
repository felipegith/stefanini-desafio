

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stefanini.Domain.Entities;
using Stefanini.Domain.Interfaces.Services;

namespace Stefanini.Infrastructure.Services.Token;

public class TokenJwt : IToken
{
    private readonly IConfiguration _configuration;
    public TokenJwt(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(Client client)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"] ?? string.Empty));
        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var claimToken = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Name, client.Name),
            new Claim(JwtRegisteredClaimNames.Email, client.BirthDate.ToString()),
            

        };
        var tokenOptions = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claimToken,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }
}

