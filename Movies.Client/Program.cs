using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Movies.Client.ApiServices;
using IdentityModel;
using Microsoft.Net.Http.Headers;
using Movies.Client.HttpHandlers;
using IdentityModel.Client;
using System.Security.AccessControl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMovieApiService, MovieApiService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = Common.Constants.IdentityServerUrl;

    options.ClientId = Common.Constants.MoviesMcvClient;
    options.ClientSecret = Common.Constants.SecretKey;
    options.ResponseType = "code id_token";// added id_token for hybrid flow

    //options.Scope.Add("openid"); auto added from openid 
    //options.Scope.Add("profile"); auto added from openid 
    options.Scope.Add("address");
    options.Scope.Add("email");
    options.Scope.Add(Common.Constants.MoviesApiScope);
    options.Scope.Add("roles");

    options.ClaimActions.MapUniqueJsonKey("role", "role");
    
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.GivenName,
        RoleClaimType = JwtClaimTypes.Role,
    };
});

// http operations start

// 1 create an HttpClient used for accessing the Movies.API
builder.Services.AddTransient<AuthenticationDelegatingHandler>();

builder.Services.AddHttpClient("MovieAPIClient", client =>
{
    client.BaseAddress = new Uri(Common.Constants.ApiGatewayUrl);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddHttpMessageHandler<AuthenticationDelegatingHandler>();


// 2 create an HttpClient used for accessing the IDP
builder.Services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri(Common.Constants.IdentityServerUrl);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

//builder.Services.AddSingleton(new ClientCredentialsTokenRequest
//{
//    Address = $"{Common.Constants.IdentityServerUrl}/connect/token",
//    ClientId = Common.Constants.MoviesClient,
//    ClientSecret = Common.Constants.SecretKey,
//    Scope = Common.Constants.MoviesApiScope
//});

builder.Services.AddHttpContextAccessor();

// http operations end

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
