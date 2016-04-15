using System;
using System.Configuration;
using System.Web.Http;
using GeoChallenger.Web.Api.Providers.Interfaces;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace GeoChallenger.Web.Api.Config
{
    public class OAuthConfig
    {
        const string TokenRelativeUrl = "/token";

        /// <summary>
        ///     Configure Owin OAuth Identity service
        /// </summary>
        /// <param name="app">Owin builder</param>
        /// <param name="configuration">Http configuration</param>
        public static void ConfigureOAuth(IAppBuilder app, HttpConfiguration configuration)
        {
            // Init oauth bearer token lifetime in days
            int tokenLifeTimeInDays;
            if (!int.TryParse(ConfigurationManager.AppSettings["UserTokenLifetimeInDays"], out tokenLifeTimeInDays)) {
                tokenLifeTimeInDays = 7;
            }

            // Init auth server options
            var authServerOptions = new OAuthAuthorizationServerOptions {
                TokenEndpointPath = new PathString(TokenRelativeUrl),
                AllowInsecureHttp = true,
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(tokenLifeTimeInDays),
                Provider = (IGeoChallengerOAuthProvider)configuration.DependencyResolver.GetService(typeof(IGeoChallengerOAuthProvider))
            };

            app.UseOAuthAuthorizationServer(authServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}