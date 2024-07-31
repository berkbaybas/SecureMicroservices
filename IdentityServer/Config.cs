using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.Events.TokenIssuedSuccessEvent;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
        [
            new Client
            {
                ClientId = Common.Constants.MoviesClient,
                ClientName = "Movies Postman Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret(Common.Constants.SecretKey.Sha256())
                },
                AllowedScopes = { Common.Constants.MoviesApiScope }
            },
            new Client // Client for mvc app
            {
                ClientId = Common.Constants.MoviesMcvClient,
                ClientName = "Movies MVC Web App",
                AllowedGrantTypes = GrantTypes.Code,
                //code flow which allows to provide to get token when login with the user credentials.
                //we will provide the login information, username and password.
                //and after that we get the token with a login and login system.
                RequirePkce = false,
                AllowRememberConsent = false,
                RedirectUris = new List<string>()
                {
                    $"{Common.Constants.IdentityServerUrl}/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>()
                {
                    $"{Common.Constants.IdentityServerUrl}/signout-callback-oidc"
                },
                ClientSecrets = new List<Secret>
                {
                    new Secret(Common.Constants.SecretKey.Sha256())
                },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Email,
                    Common.Constants.MoviesApiScope,
                    "roles"
                }
            }
        ];

        public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope(Common.Constants.MoviesApiScope, "Movie API")
        ];

        public static IEnumerable<IdentityResource> IdentityResources =>
        [
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" })
        ];

        public static List<TestUser> TestUsers =>
        [
            new TestUser
            {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "berk",
                Password = "berk",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, "berk"),
                    new Claim(JwtClaimTypes.FamilyName, "baybas")
                }
            }
        ];
    }
}