using System;
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

            app.UseOwinExceptionHandler();

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


    public class OwinExceptionHandlerMiddleware
    {
        private readonly AppFunc _next;

        public OwinExceptionHandlerMiddleware(AppFunc next)
        {
            if (next == null) {
                throw new ArgumentNullException("next");
            }

            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            try {
                await _next(environment);
            } catch (Exception ex) {
                try {

                    var owinContext = new OwinContext(environment);

                    //NLogLogger.LogError(ex, owinContext);

                    HandleException(ex, owinContext);
                    return;
                } catch (Exception) {
                    // If there's a Exception while generating the error page, re-throw the original exception.
                }
                throw;
            }
        }
        private void HandleException(Exception ex, IOwinContext context)
        {
            var request = context.Request;

            //Build a model to represet the error for the client
            //var errorDataModel = NLogLogger.BuildErrorDataModel(ex);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ReasonPhrase = "Internal Server Error";
            context.Response.ContentType = "application/json";
            //context.Response.Write(JsonConvert.SerializeObject(errorDataModel));
        }
    }

    public static class OwinExceptionHandlerMiddlewareAppBuilderExtensions
    {
        public static void UseOwinExceptionHandler(this IAppBuilder app)
        {
            app.Use<OwinExceptionHandlerMiddleware>();
        }
    }

}