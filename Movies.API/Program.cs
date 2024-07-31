using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movies.API.Data;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MoviesAPIContext>(options =>
            options.UseInMemoryDatabase("Movies"));

builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options => // JWT TOKEN FROM IDENTITY SERVER
{
    options.Authority = Common.Constants.IdentityServerUrl;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };
});
//.AddJwtBearer(options => // JWT TOKEN FROM CUSTOM JWT SERVER
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = Common.Constants.CustomJwtServerUrl,
//        ValidAudience = Common.Constants.MoviesApiUrl,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Common.Constants.SecretKey))
//    };
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id", 
        Common.Constants.MoviesClient, Common.Constants.MoviesMcvClient, Common.Constants.MoviesCustomClient));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.SeedAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
