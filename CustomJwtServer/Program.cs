using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/GenerateToken", () =>
{
    var SecretKey = Common.Constants.SecretKey;
    var Issuer = Common.Constants.CustomJwtServerUrl;
    var Audience = Common.Constants.MoviesApiUrl;

    var claims = new[]
    {
        new Claim(ClaimTypes.GivenName, "berk"),
        new Claim("client_id", Common.Constants.MoviesClient) // Add client_id claim
    };

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
          Issuer,
          Audience,
          claims,
          expires: DateTime.Now.AddMinutes(30),
          signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
});

app.Run();
