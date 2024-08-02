using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var authenticationProviderKey = "IdentityApiKey";

builder.Services.AddAuthentication()
 .AddJwtBearer(authenticationProviderKey, x =>
 {
     x.Authority = Common.Constants.IdentityServerUrl;
     
     //x.RequireHttpsMetadata = false;
     x.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateAudience = false
     };
 });

// Add Ocelot configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthorization();

await app.UseOcelot();

app.MapControllers();


app.Run();
