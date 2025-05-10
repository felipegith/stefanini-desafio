using Microsoft.Extensions.Configuration;
using NSubstitute;
using Stefanini.Domain.Entities;
using Stefanini.Domain.Interfaces.Services;
using Stefanini.Domain.Test.Fixture.Client;
using Stefanini.Domain.Test.Fixture.User;
using Stefanini.Infrastructure.Services.Token;

namespace Stefanini.Infrastructure.Test.Service.Token;

public class TokenJwtTest
{
    private readonly IToken _tokenMoq;

    public TokenJwtTest()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"JwtSettings:Secret", ClientFixture.SecretMoq},
            {"JwtSettings:Issuer", ClientFixture.IssuerMoq},
            {"JwtSettings:Audience", ClientFixture.AudienceMoq}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _tokenMoq = new TokenJwt(configuration);
    }
    [Fact]
    public void Must_GenerateToken()
    {
        var user = User.Create(UserFixture.Email, UserFixture.Password);
        var token = _tokenMoq.GenerateToken(user);

        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.Contains(".", token);
    }
}