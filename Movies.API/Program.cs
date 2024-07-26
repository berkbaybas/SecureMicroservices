using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movies.API.Data;
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
    options.Authority = "https://localhost:5005";
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
//        ValidIssuer = "https://localhost:5003",
//        ValidAudience = "https://localhost:5001",
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_generated_base64_key_here_123"))

//    };
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientIdPolicy", policy => policy.RequireClaim("client_id", "movieClient", "movies_mvc_client", "CustomMovieClient"));
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
