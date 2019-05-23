using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerHost
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "company",
                    UserClaims = { "business_id" }
                }
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource
                {
                    Name = "api",
                    DisplayName = "My API #1",
                    UserClaims = {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.GivenName,
                        JwtClaimTypes.FamilyName,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.EmailVerified,
                        JwtClaimTypes.WebSite,
                        JwtClaimTypes.Address,
                        JwtClaimTypes.ZoneInfo,
                        "document",
                        "business_id"
                    },
                    Scopes = { new Scope("api") },
                    Enabled = true
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("senha".Sha256()) },

                    RequireConsent = false,

                    AllowedScopes = { "api" }
                },

                // MVC client using hybrid flow
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("senha".Sha256()) },

                    RequireConsent = false,

                    //RedirectUris = { "http://localhost:5001/signin-oidc" },
                    //FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
                    //PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                    RedirectUris = { "https://auth.com.br/signin-oidc" },
                    FrontChannelLogoutUri = "https://auth.com.br/signout-oidc",
                    PostLogoutRedirectUris = { "https://auth.com.br/signout-callback-oidc" },

                    AllowAccessTokensViaBrowser = true,

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "company", "api" },
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "company", "api" },
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    
                }

                // SPA client using implicit flow
                //new Client
                //{
                //    ClientId = "spa",
                //    ClientName = "SPA Client",
                //    ClientUri = "http://identityserver.io",

                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,

                //    RedirectUris =
                //    {
                //        "http://localhost:5002/index.html",
                //        "http://localhost:5002/callback.html",
                //        "http://localhost:5002/silent.html",
                //        "http://localhost:5002/popup.html",
                //    },

                //    PostLogoutRedirectUris = { "http://localhost:5002/index.html" },
                //    AllowedCorsOrigins = { "http://localhost:5002" },

                //    RequireConsent = false,

                //    AllowedScopes = { "openid", "profile", "api" }
                //}
            };
        }
    }
}