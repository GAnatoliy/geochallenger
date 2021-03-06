﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using GeoChallenger.Services.Settings;
using GeoChallenger.Web.Api.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Owin;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

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
            var authenticationSettings = SettingsFactory.GetApplicationSettings();

            // Init auth server options
            var authServerOptions = new OAuthAuthorizationServerOptions {
                TokenEndpointPath = new PathString(TokenRelativeUrl),
                AllowInsecureHttp = true,
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(authenticationSettings.UserTokenLifetimeInDays),
                Provider = (GeoChallengerOAuthProvider)configuration.DependencyResolver.GetService(typeof(GeoChallengerOAuthProvider))
            };

            app.UseOAuthAuthorizationServer(authServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}